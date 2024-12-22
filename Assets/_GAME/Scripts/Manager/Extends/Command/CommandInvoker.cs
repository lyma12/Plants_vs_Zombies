
using System.Collections.Generic;

public class CommandInvoker{
    private Stack<ICommand> _commandStack = new Stack<ICommand>();
    public void ExecuteCommandEvent(){
        ICommand command = _commandStack.Pop();
        command.Execute();
    }
}