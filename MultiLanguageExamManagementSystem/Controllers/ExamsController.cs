using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;
using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Models.Entities;

namespace MultiLanguageExamManagementSystem.Controllers;

[Route("[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExamsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamResponseDto>>> GetExams()
        {
            var exams = await _unitOfWork.Repository<Exam>().GetAll()
                .Include(e => e.Creator) // Include the Creator navigation property
                .Include(e => e.ExamQuestions)
                .ThenInclude(eq => eq.Question)
                .ToListAsync();

            var examDtos = exams.Select(exam => new ExamResponseDto
            {
                ExamId = exam.ExamId,
                Title = exam.Title,
                Description = exam.Description,
                CreatorId = exam.CreatorId,
                CreatorName = exam.Creator?.Username
            }).ToList();

            return Ok(examDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ExamResponseDto>> GetExam(string id)
        {
            var exam = await _unitOfWork.Repository<Exam>().GetById(e => e.ExamId == id)
                .Include(e => e.Creator) // Include the Creator navigation property
                .Include(e => e.ExamQuestions)
                .ThenInclude(eq => eq.Question)
                .FirstOrDefaultAsync();

            if (exam == null)
            {
                return NotFound();
            }

            var examDto = new ExamResponseDto
            {
                ExamId = exam.ExamId,
                Title = exam.Title,
                Description = exam.Description,
                CreatorId = exam.CreatorId,
                CreatorName = exam.Creator?.Username
            };

            return Ok(examDto);
        }

        [HttpPost]
        public async Task<ActionResult<ExamResponseDto>> PostExam(ExamRequestDto examDto)
        {
            var exam = new Exam
            {
                Title = examDto.Title,
                Description = examDto.Description,
                CreatorId = examDto.CreatorId
            };

            _unitOfWork.Repository<Exam>().Create(exam);
            _unitOfWork.Complete();

            var responseDto = new ExamResponseDto
            {
                ExamId = exam.ExamId,
                Title = exam.Title,
                Description = exam.Description,
                CreatorId = exam.CreatorId
            };

            return CreatedAtAction("GetExam", new { id = exam.ExamId }, responseDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutExam(string id, ExamRequestDto examDto)
        {
            var exam = await _unitOfWork.Repository<Exam>().GetById(e => e.ExamId == id).FirstOrDefaultAsync();

            if (exam == null)
            {
                return NotFound();
            }

            exam.Title = examDto.Title;
            exam.Description = examDto.Description;
            exam.CreatorId = examDto.CreatorId;

            _unitOfWork.Repository<Exam>().Update(exam);
            _unitOfWork.Complete();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExam(string id)
        {
            var exam = await _unitOfWork.Repository<Exam>().GetById(e => e.ExamId == id).FirstOrDefaultAsync();
            if (exam == null)
            {
                return NotFound();
            }

            _unitOfWork.Repository<Exam>().Delete(exam);
            _unitOfWork.Complete();

            return NoContent();
        }
    }