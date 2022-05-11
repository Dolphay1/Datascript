using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataScript.StatementTypes
{
	class VariableStatement : Statement
	{
		private string variableName;
		private string variableType;
		private bool constant;
		private bool blockScoped;

		public VariableStatement(int lineNumber, string token, string variableName, string variableType, bool blockScoped) : this(lineNumber, token, variableName, variableType, blockScoped, false)
		{
		}

		public VariableStatement(int lineNumber, string token, string variableName, string variableType, bool blockScoped, bool constant) : base(lineNumber, token)
		{
			this.variableName = variableName;
			this.variableType = variableType;
			this.constant = constant;
			this.blockScoped = blockScoped;
		}

		public string getVariableName()
		{
			return variableName;
		}

		public string getVariableType()
		{
			return this.variableType;
		}

		public bool getConstant()
		{
			return constant;
		}

		public bool getBlockScoped()
		{
			return blockScoped;
		}
	}
}
