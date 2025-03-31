using Atsi.Structures.SIMPLE.Expressions;
using Atsi.Structures.SIMPLE.Interfaces;


public class ExpressionAnalyzer : IExpressionAnalyzer
    {
        public HashSet<string> GetUsedVariables(Expression expression)
        {
            var result = new HashSet<string>();
            TraverseForVariables(expression, result);
            return result;
        }

        public List<int> GetConstants(Expression expression)
        {
            var result = new List<int>();
            TraverseForConstants(expression, result);
            return result;
        }

        private void TraverseForVariables(Expression expr, HashSet<string> result)
        {
            switch (expr)
            {
                case VariableExpression varExpr:
                    result.Add(varExpr.VariableName);
                    break;
                case BinaryExpression binExpr:
                    TraverseForVariables(binExpr.Left, result);
                    TraverseForVariables(binExpr.Right, result);
                    break;
            }
        }

        private void TraverseForConstants(Expression expr, List<int> result)
        {
            switch (expr)
            {
                case ConstExpression constExpr:
                    result.Add(constExpr.Value);
                    break;
                case BinaryExpression binExpr:
                    TraverseForConstants(binExpr.Left, result);
                    TraverseForConstants(binExpr.Right, result);
                    break;
            }
        }
    }