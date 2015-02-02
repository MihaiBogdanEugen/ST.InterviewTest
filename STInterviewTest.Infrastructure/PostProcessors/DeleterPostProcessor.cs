#define DEBUG

using System;
using System.IO;
using NLog;

namespace STInterviewTest.Infrastructure.PostProcessors
{
    public class DeleterPostProcessor : IProcessPipelineTask
    {
        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();

        #region IPipelineTask Members

        /// <summary>
        /// Deletes a file
        /// The method takes and returns the same object (with additional data) for easy method chaining.
        /// WARNING: In debug mode nothing happens!
        /// </summary>
        /// <param name="task">The pipeline task witht the input data</param>
        /// <returns>The pipeline task with the input and the output data</returns>
        public PipelineTask DoWork(PipelineTask task)
        {
            if (task == null)
            {
                Logger.Error("Null task received");
                throw new ArgumentNullException("task");
            }

            #if DEBUG
                //Do not delete files, some test may depend on them
            #else
                DeleteFile(task.OriginalFilePath);
            #endif
            task.OriginalFilePath = string.Empty;

            #if DEBUG
                //Do not delete files, some test may depend on them
            #else
                DeleteFile(task.ActualFilePath);
            #endif
            task.ActualFilePath = string.Empty;
            
            Logger.Info("DeleterPostProcessor.DoWork finshed");

            return task;
        }

        #endregion IPipelineTask Members

        #region Private Members

        /// <summary>
        /// Deletes a file
        /// </summary>
        /// <param name="filePath">The file path of the file to be deleted</param>
        private static void DeleteFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath) || File.Exists(filePath) == false)
                return;

            try
            {
                File.Delete(filePath);
            }
            catch (Exception exception)
            {
                Logger.ErrorException("Cannot delete file", exception);
            }
        }

        #endregion Private Members
    }
}
