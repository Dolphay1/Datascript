using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScript.StatementTypes
{
	class StatementHolder : Statement
	{
		private List<Statement> statements;

		public StatementHolder(int lineNumber, string token, List<Statement> statements) : base(lineNumber, token)
		{
			this.statements = statements;
		}

		public StatementHolder(int lineNumber, string token) : base(lineNumber, token)
		{
			this.statements = new List<Statement>();
		}

		public List<Statement> getStatements()
		{
			return this.statements;
		}

		public void addStatement(Statement statement)
		{
			statements.Add(statement);
		}
	}
}
