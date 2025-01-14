﻿using System.Collections.Generic;
using Casino.AdminPortal.Shared;
using FluentValidation.Results;

namespace Casino.AdminPortal.Business
{
    public static class ValidationExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        public static EmployeePortalValidationResult ToValidationResult(this ValidationResult result)
        {
            IList<EmployeePortalValidationFailure> errors = new List<EmployeePortalValidationFailure>();

            foreach (ValidationFailure failure in result.Errors)
            {
                errors.Add(failure.ToValidationFailure());
            }

            return new EmployeePortalValidationResult(errors);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="failure"></param>
        /// <returns></returns>
        public static EmployeePortalValidationFailure ToValidationFailure(this ValidationFailure failure)
        {
            return new EmployeePortalValidationFailure(failure.PropertyName, failure.ErrorMessage, failure.AttemptedValue);
        }


    }

}
