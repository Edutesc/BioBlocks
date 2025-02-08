
using System.Threading.Tasks;
using System.Collections;

// somente um comentario para testar o version control
public static class TaskExtensions
{
    public static IEnumerator AsCoroutine(this Task task)
    {
        while (!task.IsCompleted)
        {
            yield return null;
        }
        if (task.IsFaulted)
        {
            throw task.Exception;
        }
    }
}
