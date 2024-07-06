using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Arr
{
    public static class TaskExtensions
    {
        public static async void CatchExceptions(this Task task)
        {
            try
            {
                await task;
            }
            catch (OperationCanceledException)
            {
                // ignore
            }
            catch (Exception exception)
            {
                Debug.LogException(exception);
            }
        }
    }
}