using System.Collections.Generic;

namespace JohaToolkit.UnityEngine.AI.UtilityBasedAI
{
    public interface IUtilityBasedAIActionChooserStrategy<TContext> where TContext : UtilityBasedContext
    {
        UtilityAIAction<TContext> ChooseAction(List<UtilityAIAction<TContext>> actions, TContext context, UtilityAIAction<TContext> currentAction = null);
    }
}