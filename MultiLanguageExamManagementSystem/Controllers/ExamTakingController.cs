using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;
using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Models.Entities;

namespace MultiLanguageExamManagementSystem.Controllers;

    [Route("[controller]")]
    [ApiController]
    public class ExamTakingController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        // private readonly IEmailService _emailService;
        //
        // public ExamTakingController(IUnitOfWork unitOfWork, IEmailService emailService)
        // {
        //     _unitOfWork = unitOfWork;
        //     _emailService = emailService;
        // }
        
        public ExamTakingController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("questions/{examId}")]
        public async Task<ActionResult<IEnumerable<QuestionResponseDto>>> GetQuestions(string examId)
        {
            var exam = await _unitOfWork.Repository<Exam>()
                .GetById(e => e.ExamId == examId)
                .Include(e => e.ExamQuestions)
                .ThenInclude(eq => eq.Question)
                .FirstOrDefaultAsync();

            if (exam == null)
            {
                return NotFound();
            }

            var questionDtos = exam.ExamQuestions.Select(eq => new QuestionResponseDto
            {
                QuestionId = eq.Question.QuestionId,
                Text = eq.Question.Text,
                PossibleAnswers = eq.Question.PossibleAnswers
            }).ToList();

            return Ok(questionDtos);
        }

        [HttpPost("submit")]
        public async Task<ActionResult<ExamResultDto>> SubmitExam(ExamSubmissionDto submissionDto)
        {
            var exam = await _unitOfWork.Repository<Exam>()
                .GetById(e => e.ExamId == submissionDto.ExamId)
                .Include(e => e.ExamQuestions)
                .ThenInclude(eq => eq.Question)
                .FirstOrDefaultAsync();

            if (exam == null)
            {
                return NotFound();
            }

            int correctAnswers = 0;
            foreach (var answer in submissionDto.Answers)
            {
                var question = exam.ExamQuestions.FirstOrDefault(eq => eq.QuestionId == answer.Key)?.Question;
                if (question != null && question.Answer == answer.Value)
                {
                    correctAnswers++;
                }
            }

            int totalQuestions = exam.ExamQuestions.Count;
            var result = new ExamResultDto
            {
                ExamId = submissionDto.ExamId,
                UserId = submissionDto.UserId,
                TotalQuestions = totalQuestions,
                CorrectAnswers = correctAnswers,
                Score = (double)correctAnswers / totalQuestions * 100
            };

            // Optionally save results to the database
            var takenExam = new TakenExam
            {
                TakenExamId = Guid.NewGuid().ToString(),
                ExamId = submissionDto.ExamId,
                StudentId = submissionDto.UserId,
                TakenOn = DateTime.UtcNow,
                IsCompleted = true,
                Score = result.Score
            };

            _unitOfWork.Repository<TakenExam>().Create(takenExam);
            _unitOfWork.Complete();

            // // Send results via email
            // var user = await _unitOfWork.Repository<User>().GetById(u => u.UserId == submissionDto.UserId).FirstOrDefaultAsync();
            // if (user != null)
            // {
            //     await _emailService.SendExamResult(user.Email, result);
            // }

            return Ok(result);
        }
    }