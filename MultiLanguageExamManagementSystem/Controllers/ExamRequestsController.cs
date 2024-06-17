using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;
using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Models.Entities;

namespace MultiLanguageExamManagementSystem.Controllers;

    [Route("[controller]")]
    [ApiController]
    public class ExamRequestsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExamRequestsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("exams")]
        public async Task<ActionResult<IEnumerable<ExamResponseDto>>> GetExams()
        {
            var exams = await _unitOfWork.Repository<Exam>().GetAll().ToListAsync();
            var examDtos = exams.Select(e => new ExamResponseDto
            {
                ExamId = e.ExamId,
                Title = e.Title,
                Description = e.Description,
                CreatorId = e.CreatorId,
                CreatorName = e.Creator.Username
            }).ToList();

            return Ok(examDtos);
        }

        [HttpPost("request")]
        public async Task<ActionResult> RequestExam(ExamRequestCreateDto requestDto)
        {
            var existingRequests = await _unitOfWork.Repository<ExamRequest>()
                .GetByCondition(r => r.ExamId == requestDto.ExamId && r.UserId == requestDto.UserId)
                .CountAsync();

            if (existingRequests >= 3)
            {
                return BadRequest("No more attempts left.");
            }

            var examRequest = new ExamRequest
            {
                ExamId = requestDto.ExamId,
                UserId = requestDto.UserId
            };

            _unitOfWork.Repository<ExamRequest>().Create(examRequest);
            _unitOfWork.Repository<ExamRequest>()
                .GetByCondition(x => x.ExamId == requestDto.ExamId && x.UserId == requestDto.UserId)
                .Select(x => x.ExamRequestId).FirstOrDefault();
            _unitOfWork.Complete();

            return Ok();
        }

        [HttpGet("approved")]
        public async Task<ActionResult<IEnumerable<ExamResponseDto>>> GetApprovedExams(string userId)
        {
            var approvedRequests = await _unitOfWork.Repository<ExamRequest>()
                .GetByCondition(r => r.UserId == userId && r.IsApproved)
                .Include(r => r.Exam)
                .ToListAsync();

            var examDtos = approvedRequests.Select(r => new ExamResponseDto
            {
                ExamId = r.ExamId,
                Title = r.Exam.Title,
                Description = r.Exam.Description,
                CreatorId = r.Exam.CreatorId,
                CreatorName = r.Exam.Creator.Username
            }).ToList();

            return Ok(examDtos);
        }

        [HttpPost("approve/{requestId}")]
        public async Task<ActionResult> ApproveRequest(string requestId)
        {
            var examRequest = await _unitOfWork.Repository<ExamRequest>()
                .GetById(r => r.ExamRequestId == requestId)
                .FirstOrDefaultAsync();

            if (examRequest == null)
            {
                return NotFound();
            }

            examRequest.IsApproved = true;
            _unitOfWork.Repository<ExamRequest>().Update(examRequest);
            _unitOfWork.Complete();

            return Ok();
        }
    }