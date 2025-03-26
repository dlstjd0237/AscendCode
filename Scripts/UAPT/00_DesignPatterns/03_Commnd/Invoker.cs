using UnityEngine;

public class Invoker : MonoBehaviour
{
    public void ExecuteCommand(Command command)
    {

        command.Execute(); //�޾ƿ� Ŀ�ǵ� ����
    }
    
}
