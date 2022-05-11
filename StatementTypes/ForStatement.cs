using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScript.StatementTypes
{
	class ForStatement : ConditionalStatement
	{
		private VariableStatement? variableStatement;
		private Statement? successCondition;

		public ForStatement(int lineNumber, string token, List<Statement> statements, string condition, VariableStatement? variableStatement, Statement successCondition) : base(lineNumber, token, statements, condition)
		{
			this.variableStatement = variableStatement;
			this.successCondition = successCondition;
		}
	}
}
