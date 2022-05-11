using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScript.StatementTypes
{
	class FunctionStatement : StatementHolder
	{
		private Dictionary<string, ParameterStatement> variables;
		private string functionName;


		public FunctionStatement(int lineNumber, string token, string functionName) : base(lineNumber, token)
		{
			this.variables = new Dictionary<string, ParameterStatement>();
			this.functionName = functionName;
		}

		public FunctionStatement(int lineNumber, string token, string functionName, List<Statement> statements) : base(lineNumber, token, statements)
		{
			this.variables = new Dictionary<string, ParameterStatement>();
			this.functionName = functionName;
		}

		public void addParameter(string name, ParameterStatement parameter)
		{
			variables.Add(name, parameter);
		}
	}

	class FunctionInformation
	{
		protected string functionName;
		protected string[] functionVariableTypes;

		public FunctionInformation (string functionName, string[] functionVariableTypes)
		{
			this.functionName= functionName;
			this.functionVariableTypes = functionVariableTypes;
		}

		public class EqualityComparer : IEqualityComparer<FunctionInformation>
		{
			bool IEqualityComparer<FunctionInformation>.Equals(FunctionInformation? x, FunctionInformation? y)
			{
				return (x.functionName == y.functionName && Enumerable.SequenceEqual(x.functionVariableTypes, y.functionVariableTypes));
			}

			int IEqualityComparer<FunctionInformation>.GetHashCode(FunctionInformation obj)
			{
				return obj.functionVariableTypes.Length;
			}
		}
	}
}
