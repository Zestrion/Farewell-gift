using System;
using System.Collections.Generic;
using UnityEngine;

namespace Survivors.AI
{
    [System.Serializable]
    public class StateMachine : MonoBehaviour
    {
        [System.Serializable]
        private class Transition
        {
            public Func<bool> Condition { get; }
            public State To { get; }

            public Transition(State to, Func<bool> condition)
            {
                To = to;
                Condition = condition;
            }
        }

        public State CurrentState => currentState;
        private State currentState;

        private Dictionary<State, List<Transition>> transitions = new Dictionary<State, List<Transition>>();
        private List<Transition> currentTransitions = new List<Transition>();
        private List<Transition> anyTransitions = new List<Transition>();

        private static List<Transition> emptyTransitions = new List<Transition>(0);
        private bool isStarted;

        /// <summary>
        /// Add all required transitions and start state machine
        /// </summary>
        public void StartStateMachine(State _firstState)
        {
            SetState(_firstState);
            isStarted = true;
        }

        public void AddTransition(State _from, State _to, Func<bool> _predicate)
        {
            if (transitions.TryGetValue(_from, out List<Transition> _transitions) == false)
            {
                _transitions = new List<Transition>();
                transitions[_from] = _transitions;
            }

            _transitions.Add(new Transition(_to, _predicate));
        }

        public void AddAnyTransition(State _state, Func<bool> _predicate)
        {
            anyTransitions.Add(new Transition(_state, _predicate));
        }

        private void SetState(State _state)
        {
            if (_state == currentState)
            {
                return;
            }

            currentState?.OnExit();
            currentState = _state;

            transitions.TryGetValue(currentState, out currentTransitions);
            if (currentTransitions == null)
            {
                currentTransitions = emptyTransitions;
            }

            currentState.OnEnter();
        }

        private Transition GetTransition()
        {
            foreach (Transition transition in anyTransitions)
            {
                if (transition.Condition() && transition.To.CanEnter())
                {
                    return transition;
                }
            }

            foreach (Transition transition in currentTransitions)
            {
                if (transition.Condition() && transition.To.CanEnter())
                {
                    return transition;
                }
            }

            return null;
        }

        private void Tick()
        {
            Transition transition = GetTransition();
            if (transition != null)
            {
                SetState(transition.To);
            }
        }

        protected virtual void Update()
        {
            if(isStarted)
            {
                Tick();
            }
        }
    }
}
