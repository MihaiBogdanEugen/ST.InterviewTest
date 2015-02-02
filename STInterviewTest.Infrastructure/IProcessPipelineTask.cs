
namespace STInterviewTest.Infrastructure
{
    public interface IProcessPipelineTask
    {
        /// <summary>
        /// Does processing on a pipeline task.
        /// The method takes and returns the same object (with additional data) for easy method chaining.
        /// </summary>
        /// <param name="task">The pipeline task witht the input data</param>
        /// <returns>The pipeline task with the input and the output data</returns>
        PipelineTask DoWork(PipelineTask task);
    }
}
