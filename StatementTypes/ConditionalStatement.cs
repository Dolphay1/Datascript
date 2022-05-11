using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScript.StatementTypes
{
	class ConditionalStatement : StatementHolder
	{
		private string condition;

		public ConditionalStatement(int lineNumber, string token, List<Statement> statements, string condition) : base(lineNumber, token, statements)
		{
			this.condition = condition;
		}

		public string getCondition()
		{
			return this.condition;
		}
	}
}
