using System;
using System.IO;
using System.IO.Compression;
using NLog;

namespace STInterviewTest.Infrastructure.PreProcessors
{
    public class UnzipperPreProcessor : IProcessPipelineTask
    {
        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();

        #region IPipelineTask Members

        /// <summary>
        /// Unzipps a zip archive that contains exactly one file
        /// The method takes and returns the same object (with additional data) for easy method chaining.
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

            task.ActualFilePath = UnzipFile(task.OriginalFilePath);

            Logger.Info("UnzipperPreProcessor.DoWork finshed");

            return task;
        }

        #endregion IPipelineTask Members

        #region Private Members

        /// <summary>
        /// Unzipps a zip archive that contains exactly one file
        /// </summary>
        /// <param name="filePath">The file path of the archive</param>
        /// <returns>The file path of the extracted file</returns>
        public static string UnzipFile(string filePath)
        {
            var fileType = filePath.GetFileType();

            var directoryName = Path.GetDirectoryName(filePath);
            if (string.IsNullOrEmpty(directoryName))
            {
                Logger.Error("Cannot find directory name of path: [{0}]", filePath);
                throw new DirectoryNotFoundException("Cannot find directory name of path: [" + filePath + "]");
            }

            switch (fileType)
            {
                case FileTypes.ZipFile:
                {
                    try
                    {
                        using (var zipArchive = ZipFile.Open(filePath, ZipArchiveMode.Read))
                        {
                            if (zipArchive.Entries.Count != 1)
                            {
                                Logger.Error("Archives with multiple files are not supported!");
                                throw new NotSupportedException("Archives with multiple files are not supported!");
                            }

                            var entry = zipArchive.Entries[0];

                            var actualFilePath = Path.Combine(directoryName, entry.FullName);
                            
                            if (File.Exists(actualFilePath))
                                File.Delete(actualFilePath);

                            entry.ExtractToFile(actualFilePath);

                            return actualFilePath;
                        }
                    }
                    catch (Exception exception)
                    {
                        Logger.ErrorException("Cannot unzip file", exception);
                    }
                    break;
                }
                case FileTypes.Unknown:
                {
                    Logger.Error("Unrecognized file type: [{0}]", filePath);
                    throw new NotSupportedException("Unrecognized file type: [" + filePath + "]");
                }
            }

            return filePath;
        }

        #endregion Private Members
    }
}
