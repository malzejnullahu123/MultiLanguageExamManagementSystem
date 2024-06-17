using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;
using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Models.Entities;

namespace MultiLanguageExamManagementSystem.Controllers;

[Route("[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponseDto>>> GetUsers()
        {
            var users = await _unitOfWork.Repository<User>().GetAll().ToListAsync();
            var response = new List<UserResponseDto>();
            foreach (var user in users)
            {
                response.Add(new UserResponseDto
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Role = user.Role,
                    CreatedExams = user.CreatedExams,
                    TakenExams = user.TakenExams
                });
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDto>> GetUser(string id)
        {
            var user = await _unitOfWork.Repository<User>().GetById(u => u.UserId == id).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            var userResponse = new UserResponseDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Role = user.Role,
                CreatedExams = user.CreatedExams,
                TakenExams = user.TakenExams
            };

            return Ok(userResponse);
        }

        [HttpPost]
        public async Task<ActionResult<UserResponseDto>> PostUser(UserRequestDto user)
        {
            var role = string.IsNullOrEmpty(user.Role) ? "student" : user.Role;
            var newUser = new User()
            {
                UserId = Guid.NewGuid().ToString(),
                Username = user.Password,
                Password = user.Username,
                Role = role
            };
            _unitOfWork.Repository<User>().Create(newUser);
            _unitOfWork.Complete();

            var userResponse = new UserResponseDto
            {
                UserId = newUser.UserId,
                Username = newUser.Username,
                Role = newUser.Role
            };

            return Ok(userResponse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, UserRequestDto user)
        {
            var userOnDb = _unitOfWork.Repository<User>().GetByCondition(x => x.UserId == id).FirstOrDefault();
            if (id != userOnDb.UserId)
            {
                return BadRequest();
            }

            userOnDb.Username = user.Username;
            userOnDb.Password = user.Password;
            userOnDb.Role = user.Role;
            _unitOfWork.Repository<User>().Update(userOnDb);

            try
            {
                _unitOfWork.Complete();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _unitOfWork.Repository<User>().GetById(u => u.UserId == id).FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }

            _unitOfWork.Repository<User>().Delete(user);
            _unitOfWork.Complete();

            return NoContent();
        }

        private bool UserExists(string id)
        {
            return _unitOfWork.Repository<User>().GetById(u => u.UserId == id).Any();
        }
}