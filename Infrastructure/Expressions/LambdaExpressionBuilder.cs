using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Infrastructure.Expressions
{
    public interface IExpressionAddFirstParameter
    {
        IExpressionAccessFirstProperty AddParameter(Type elementType);
        IExpressionAccessFirstProperty AddParameter<T>();
    }

    public interface IExpressionAccessFirstProperty : IFinishAccessBuilder
    {
        IThenAccessPropertyBuilder AccessProperty(string propertyName);
    }

    public interface IThenAccessPropertyBuilder : IFinishAccessBuilder
    {
        IThenAccessPropertyBuilder ThenProperty(string propertyName);
    }

    public interface IFinishAccessBuilder
    {
        IBuildingFinished FinishAccess();
    }

    public interface IBuildingFinished
    {
        BuilderResult Build();
    }

    public class BuilderResult
    {
        public IList<string> Errors { get; set; } = new List<string>();
        
        public bool IsSuccessfull { get; set; }

        public LambdaExpression Result { get; set; } = Expression.Lambda(Expression.Default(typeof(void)), false);
    }

    public class LambdaExpressionBuilder : IExpressionAddFirstParameter, IExpressionAccessFirstProperty, IThenAccessPropertyBuilder, IFinishAccessBuilder, IBuildingFinished
    {
        private ParameterExpression _parameterExpression;
        private MemberExpression _body;
        private readonly BuilderResult _result;

        private const BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase;

        private LambdaExpressionBuilder()
        {
            _parameterExpression = null!;
            _body = null!;
            _result = new();
        }

        public static IExpressionAddFirstParameter Create() => new LambdaExpressionBuilder();

        public IExpressionAccessFirstProperty AddParameter(Type elementType)
        {
            _parameterExpression = Expression.Parameter(elementType);
            return this;
        }

        public IExpressionAccessFirstProperty AddParameter<T>() => AddParameter(typeof(T));

        public IThenAccessPropertyBuilder AccessProperty(string propertyName)
        {
            var pInfo = _parameterExpression.Type.GetProperty(propertyName, flags);
            if (pInfo is null)
            {
                _result.Errors.Add($"'{propertyName}' is not member of type '{_parameterExpression.Type}'");
                _result.IsSuccessfull = false;
                return this;
            }

            _body = Expression.Property(_parameterExpression, pInfo);
            _result.IsSuccessfull = true;

            return this;
        }

        public IThenAccessPropertyBuilder ThenProperty(string propertyName)
        {
            if (!_result.IsSuccessfull)
            {
                return this;
            }

            var pInfo = _body.Type.GetProperty(propertyName, flags);
            if (pInfo is null)
            {
                _result.Errors.Add($"'{propertyName}' is not member of type '{_parameterExpression.Type}'");
                _result.IsSuccessfull = false;
                return this;
            }

            _body = Expression.Property(_body, pInfo);
            _result.IsSuccessfull = true;

            return this;
        }

        public BuilderResult Build()
        {
            if (_result.IsSuccessfull)
            {
                return new()
                {
                    IsSuccessfull = true,
                    Result = Expression.Lambda(_body, _parameterExpression)
                };
            }

            return new()
            {
                IsSuccessfull = _result.IsSuccessfull,
                Errors = _result.Errors.ToList()
            };
        }

        public IBuildingFinished FinishAccess()
        {
            return this;
        }
    }
}
