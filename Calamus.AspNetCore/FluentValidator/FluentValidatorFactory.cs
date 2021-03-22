using Calamus.Ioc;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calamus.AspNetCore.FluentValidator
{
    /// <summary>
    /// 获取 FluentValidator 工厂
    /// </summary>
    public class FluentValidatorFactory : ValidatorFactoryBase
    {
        public override IValidator CreateInstance(Type validatorType)
        {
            try
            {
                var validator = EngineContext.Current.GetService(validatorType);
                return (IValidator)validator;
            }
            catch
            {
                return null;
            }
        }
    }
}
