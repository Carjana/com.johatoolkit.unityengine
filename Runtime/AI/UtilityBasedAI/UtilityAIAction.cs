using System.Collections.Generic;
using System.Linq;

namespace JohaToolkit.UnityEngine.AI.UtilityBasedAI
{
    public abstract class UtilityAIAction<TContext>
    {
        protected readonly List<Scorer<TContext>> Scorers = new();
        protected readonly TContext Context;
        protected UtilityAIAction(TContext context)
        {
            Context = context;
        }
        
        public void AddScorer(Scorer<TContext> scorer) => Scorers.Add(scorer);

        public void AddScorers(IEnumerable<Scorer<TContext>> scorers) => Scorers.AddRange(scorers);

        public float EvaluateScore(TContext context) => Scorers.Sum(scorer => scorer.EvaluateScore(context));

        public abstract void StartAction();

        public abstract void UpdateAction(float deltaTime);

        public abstract void EndAction();
    }
}