using LifeEcommerce.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MultiLanguageExamManagementSystem.Data.UnitOfWork;
using MultiLanguageExamManagementSystem.Models.Dtos;
using MultiLanguageExamManagementSystem.Models.Entities;
using MultiLanguageExamManagementSystem.Services.IServices;
using System.Globalization;

namespace MultiLanguageExamManagementSystem.Services
{
    public class CultureService : ICultureService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CultureService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Your code here

        #region String Localization

        // String localization methods implementation here

        #endregion

        #region Languages

        // language methods implementation here

        #endregion

        #region Localization Resources

        // localization resource methods implementation here

        #endregion
    }
}
