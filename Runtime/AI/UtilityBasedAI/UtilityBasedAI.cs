using System.Collections.Generic;

namespace JohaToolkit.UnityEngine.AI.UtilityBasedAI
{
    public class UtilityBasedAI<TContext> where TContext : UtilityBasedContext
    {
        private UtilityBasedAI(){}
        public List<UtilityAIAction<TContext>> Actions { get; } = new();
        public UtilityAIAction<TContext> CurrentUtilityAIAction {get; protected set; }
        private IUtilityBasedAIActionChooserStrategy<TContext> _actionChooserStrategy;
        public TContext Context { get; protected set; }
        
        public void AddAction(UtilityAIAction<TContext> utilityAIAction) => Actions.Add(utilityAIAction);

        public void AddActions(IEnumerable<UtilityAIAction<TContext>> actions) => Actions.AddRange(actions);

        public void SetActionChooserStrategy(IUtilityBasedAIActionChooserStrategy<TContext> actionChooserStrategy) => _actionChooserStrategy = actionChooserStrategy;

        public void SetContext(TContext context) => Context = context;
        
        public void Update(float deltaTime)
        {
            UtilityAIAction<TContext> chosenUtilityAIAction = _actionChooserStrategy.ChooseAction(Actions, Context, CurrentUtilityAIAction);
            if (CurrentUtilityAIAction != chosenUtilityAIAction)
            {
                CurrentUtilityAIAction?.EndAction();
                CurrentUtilityAIAction = chosenUtilityAIAction;
                CurrentUtilityAIAction.StartAction();
            }
            
            CurrentUtilityAIAction?.UpdateAction(deltaTime);
        }

        public class Builder
        {
            private UtilityBasedAI<TContext> _utilityBasedAI;
            
            public static Builder BeginBuilder()
            {
                Builder builder = new Builder();
                builder._utilityBasedAI = new UtilityBasedAI<TContext>();
                return builder;
            }

            public Builder WithContext(TContext context)
            {
                _utilityBasedAI.SetContext(context);
                return this;
            }
            
            public Builder WithStrategy(IUtilityBasedAIActionChooserStrategy<TContext> actionChooserStrategy)
            {
                _utilityBasedAI.SetActionChooserStrategy(actionChooserStrategy);
                return this;
            }

            public Builder WithDefaultStrategy(float threshold)
            {
                _utilityBasedAI.SetActionChooserStrategy(new HighestPriorityChooser<TContext>(threshold));
                return this;
            }
            
            public Builder WithAction(UtilityAIAction<TContext> utilityAIAction)
            {
                _utilityBasedAI.AddAction(utilityAIAction);
                return this;
            }
            
            public Builder WithActions(IEnumerable<UtilityAIAction<TContext>> actions)
            {
                _utilityBasedAI.AddActions(actions);
                return this;
            }
            
            public UtilityBasedAI<TContext> Build()
            {
                if (_utilityBasedAI._actionChooserStrategy == null)
                {
                    WithDefaultStrategy(0);
                }
                return _utilityBasedAI;
            }
        }
    }

    public abstract class UtilityBasedContext
    {
        
    }
    
}
