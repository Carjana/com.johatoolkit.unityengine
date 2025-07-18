using System;
using System.Collections.Generic;

namespace JohaToolkit.UnityEngine.AI.UtilityBasedAI
{
    public class HighestPriorityChooser<TContext> : IUtilityBasedAIActionChooserStrategy<TContext> where TContext : UtilityBasedContext
    {
        public float Threshold;
        public HighestPriorityChooser(float threshold)
        {
            Threshold = threshold;
        }
        
        public UtilityAIAction<TContext> ChooseAction(List<UtilityAIAction<TContext>> actions, TContext context, UtilityAIAction<TContext> currentAction)
        {
            if (actions == null || actions.Count == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(actions), "Actions cannot be null or empty");
            }

            UtilityAIAction<TContext> bestUtilityAIAction = actions[0];
            float bestScore = bestUtilityAIAction.EvaluateScore(context);

            foreach (UtilityAIAction<TContext> action in actions)
            {
                float score = action.EvaluateScore(context);
                if (!(score > bestScore))
                    continue;
                bestScore = score;
                bestUtilityAIAction = action;
            }

            if (currentAction != null && bestScore > currentAction.EvaluateScore(context) + Threshold)
                bestUtilityAIAction = currentAction;
            
            return bestUtilityAIAction;
        }
    }
}