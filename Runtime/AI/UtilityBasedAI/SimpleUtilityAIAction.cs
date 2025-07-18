using System;
using System.Collections.Generic;

namespace JohaToolkit.UnityEngine.AI.UtilityBasedAI
{
    public class SimpleUtilityAIAction<TContext> : UtilityAIAction<TContext>
    {

        protected Action<TContext> OnStartAction;
        protected Action<TContext, float> OnUpdateAction;
        protected Action<TContext> OnEndAction;
        
        private SimpleUtilityAIAction(TContext context) : base(context) {}

        public override void StartAction() => OnStartAction?.Invoke(Context);

        public override void UpdateAction(float deltaTime) => OnUpdateAction?.Invoke(Context, deltaTime);

        public override void EndAction() => OnEndAction?.Invoke(Context);

        public class Builder
        {
            private SimpleUtilityAIAction<TContext> _utilityAIAction;
            public static Builder BeginBuilder(TContext context)
            {
                Builder builder = new();
                builder._utilityAIAction = new SimpleUtilityAIAction<TContext>(context);
                return builder;
            }
            
            public Builder WithStartAction(Action<TContext> onStartAction)
            {
                _utilityAIAction.OnStartAction = onStartAction;
                return this;
            }
            
            public Builder WithUpdateAction(Action<TContext, float> onUpdateAction)
            {
                _utilityAIAction.OnUpdateAction = onUpdateAction;
                return this;
            }
            
            public Builder WithEndAction(Action<TContext> onEndAction)
            {
                _utilityAIAction.OnEndAction = onEndAction;
                return this;
            }
            
            public Builder WithScorer(Scorer<TContext> scorer)
            {
                _utilityAIAction.AddScorer(scorer);
                return this;
            }
            
            public Builder WithScorers(IEnumerable<Scorer<TContext>> scorers)
            {
                _utilityAIAction.AddScorers(scorers);
                return this;
            }
            
            public SimpleUtilityAIAction<TContext> Build() => _utilityAIAction;
        }
    }
}