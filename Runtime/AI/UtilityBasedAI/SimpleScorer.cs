using System;

namespace JohaToolkit.UnityEngine.AI.UtilityBasedAI
{
    public class SimpleScorer<TContext> : Scorer<TContext>
    {
        private readonly Func<TContext, float> _scoreFunction;

        public SimpleScorer(Func<TContext, float> scoreFunction)
        {
            _scoreFunction = scoreFunction;
        }

        public override float EvaluateScore(TContext context) => _scoreFunction(context);
    }
}