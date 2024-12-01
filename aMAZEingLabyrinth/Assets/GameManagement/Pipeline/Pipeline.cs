using System;
using System.Collections.Generic;

namespace GamePipeline
{
    public class Pipeline
    {
        public event Action OnFinished;

        private readonly List<Task> _tasks = new List<Task>();

        private int _currentIndex = 0;

        public void AddTask(Task task)
        {
            _tasks.Add(task);
        }

        public void Clear()
        {
            _tasks.Clear();
        }

        public void Run()
        {
            if (_tasks.Count > 0)
            {
                _tasks[_currentIndex].Run(OnTaskFinished);
            }
        }

        public void OnTaskFinished()
        {
            _currentIndex++;
            if (_currentIndex >= _tasks.Count)
            {
                _currentIndex = 0;

                OnFinished?.Invoke();

                return;
            }

            _tasks[_currentIndex].Run(OnTaskFinished);
        }
    }
}