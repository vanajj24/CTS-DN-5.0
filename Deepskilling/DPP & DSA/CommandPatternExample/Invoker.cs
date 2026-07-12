using System;
using System.Collections.Generic;

namespace CommandPatternExample
{
    public class RemoteControl
    {
        private ICommand? _command;
        private readonly Dictionary<string, ICommand> _commandHistory;
        private readonly List<ICommand> _undoStack;

        public RemoteControl()
        {
            _commandHistory = new Dictionary<string, ICommand>();
            _undoStack = new List<ICommand>();
        }

        public void SetCommand(ICommand command)
        {
            _command = command;
        }

        public void PressButton()
        {
            if (_command == null)
            {
                Console.WriteLine("No command assigned to this button");
                return;
            }

            Console.WriteLine("\n--- Pressing Remote Button ---");
            _command.Execute();
            _undoStack.Add(_command);
            Console.WriteLine("--- Button Pressed ---\n");
        }

        public void PressUndoButton()
        {
            if (_undoStack.Count == 0)
            {
                Console.WriteLine("No commands to undo");
                return;
            }

            Console.WriteLine("\n--- Pressing Undo Button ---");
            ICommand lastCommand = _undoStack[_undoStack.Count - 1];
            _undoStack.RemoveAt(_undoStack.Count - 1);
            
            Console.WriteLine($"Undoing last command: {lastCommand.GetType().Name}");
            Console.WriteLine("--- Undo Complete ---\n");
        }

        public void RegisterButton(string buttonName, ICommand command)
        {
            _commandHistory[buttonName] = command;
            Console.WriteLine($"Button '{buttonName}' registered with command: {command.GetType().Name}");
        }

        public void PressButtonByName(string buttonName)
        {
            if (!_commandHistory.ContainsKey(buttonName))
            {
                Console.WriteLine($"Button '{buttonName}' not found");
                return;
            }

            Console.WriteLine($"\n--- Pressing Button: {buttonName} ---");
            _commandHistory[buttonName].Execute();
            Console.WriteLine("--- Button Pressed ---\n");
        }

        public void ShowCommandHistory()
        {
            Console.WriteLine("\n=== Command History ===");
            foreach (var item in _commandHistory)
            {
                Console.WriteLine($"Button: {item.Key} -> Command: {item.Value.GetType().Name}");
            }
            Console.WriteLine("=====================\n");
        }

        public int GetUndoStackSize()
        {
            return _undoStack.Count;
        }
    }
}