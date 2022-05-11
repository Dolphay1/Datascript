using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScript.StatementTypes
{
	class ParameterStatement : VariableStatement
	{
		private bool optional;

		public ParameterStatement(int lineNumber, string token, string variableName, string variableType) : this(lineNumber, token, variableName, variableType, false)
		{
		}

		public ParameterStatement(int lineNumber, string token, string variableName, string variableType, bool optional) : base(lineNumber, token, variableName, variableType, false)
		{
			this.optional = optional;
		}
	}
}
