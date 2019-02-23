using System;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Text;

namespace RevStackCore.Aws
{
    public static class Extensions
    {
        public static object GetResult(this Task task)
        {
            try
            {
                if (!task.IsCompleted)
                    task.Wait();

                if (task is Task<object> taskObj)
                    return taskObj.Result;

                var taskType = task.GetType();
                if (!taskType.IsGenericType || taskType.FullName.Contains("VoidTaskResult"))
                    return null;

                var props = TypeProperties.Get(taskType);
                var fn = props.GetPublicGetter("Result");
                return fn?.Invoke(task);
            }
            catch (TypeAccessException)
            {
                return null; //return null for void Task's
            }
            catch (Exception ex)
            {
                throw ex.UnwrapIfSingleException();
            }
        }

        public static T GetResult<T>(this Task<T> task)
        {
            return (T)((Task)task).GetResult();
        }
    }
}
