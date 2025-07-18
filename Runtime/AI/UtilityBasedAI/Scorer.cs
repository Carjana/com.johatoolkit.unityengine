namespace JohaToolkit.UnityEngine.AI.UtilityBasedAI
{
    public abstract class Scorer<TContext>
    {
        public abstract float EvaluateScore(TContext context);
    }
}