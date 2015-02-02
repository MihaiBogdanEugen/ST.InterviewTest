using System;
using System.Collections.Generic;
using System.Linq;
using NLog;
using STInterviewTest.Infrastructure.Analyzers;
using STInterviewTest.Infrastructure.PostProcessors;
using STInterviewTest.Infrastructure.PreProcessors;
using STInterviewTest.Infrastructure.TextExtractors;

namespace STInterviewTest.Infrastructure
{
    public class Pipeline
    {
        private readonly static Logger Logger = LogManager.GetCurrentClassLogger();

        private readonly List<Func<PipelineTask, PipelineTask>> _preProcessorTasks;
        private readonly List<Func<PipelineTask, PipelineTask>> _processorTasks; 
        private readonly List<Func<PipelineTask, PipelineTask>> _postProcessorTasks;
 
        public Pipeline()
        {
            this._preProcessorTasks = new List<Func<PipelineTask, PipelineTask>>();
            this._processorTasks = new List<Func<PipelineTask, PipelineTask>>();
            this._postProcessorTasks = new List<Func<PipelineTask, PipelineTask>>();
        }

        public IEnumerable<Func<PipelineTask, PipelineTask>> Tasks
        {
            get
            {
                return this._preProcessorTasks
                    .Concat(this._processorTasks)
                    .Concat(this._postProcessorTasks); 
            }
        } 

        public void AddPreProcessorTask(Func<PipelineTask, PipelineTask> task, int index = -1)
        {
            if (index == -1 || index >= this._preProcessorTasks.Count)
                this._preProcessorTasks.Add(task);
            else
                this._preProcessorTasks.Insert(index, task);
        }

        public void AddProcessorTask(Func<PipelineTask, PipelineTask> task, int index = -1)
        {
            if (index == -1 || index >= this._processorTasks.Count)
                this._processorTasks.Add(task);
            else
                this._processorTasks.Insert(index, task);        
        }

        public void AddPostProcessorTask(Func<PipelineTask, PipelineTask> task, int index = -1)
        {
            if (index == -1 || index >= this._postProcessorTasks.Count)
                this._postProcessorTasks.Add(task);
            else            
                this._postProcessorTasks.Insert(index, task);            
        }

        public static Pipeline Setup()
        {
            var pipeline = new Pipeline();

            pipeline.AddPreProcessorTask((new UnzipperPreProcessor()).DoWork);
            pipeline.AddProcessorTask((new OpinionAnalyzer()).DoWork);
            pipeline.AddPostProcessorTask((new DeleterPostProcessor()).DoWork);

            return pipeline;
        }

        public string ProcessPdfFile(string filePath)
        {
            return this.ProcessFile(filePath, FileTypes.PdfFile);
        }

        public string ProcessTxtFile(string filePath)
        {
            return this.ProcessFile(filePath, FileTypes.TxtFile);
        }

        private string ProcessFile(string actualFilePath, FileTypes originalFileType)
        {
            var actualFileType = actualFilePath.GetFileType();
            if (actualFileType == FileTypes.Unknown)
            {
                Logger.Error("Unsupported file type: [{0}]", actualFilePath);
                throw new NotSupportedException("Unsupported file type: [" + actualFilePath + "]");
            }

            if (originalFileType != FileTypes.PdfFile && originalFileType != FileTypes.TxtFile)
            {
                Logger.Error("Unsupported original file type: [{0}]", originalFileType);
                throw new NotSupportedException("Unsupported original file type: [" + originalFileType + "]");
            }

            if (originalFileType == FileTypes.PdfFile)
                this.AddProcessorTask((new PdfFileTextExtractor()).DoWork, 0);
            else
                this.AddProcessorTask((new TxtFileTextExtractor()).DoWork, 0);

            var pipelineTask = new PipelineTask {OriginalFilePath = actualFilePath};

            foreach(var task in this.Tasks)
            {
                pipelineTask = task.Invoke(pipelineTask);
            }

            return pipelineTask.AnalyzerResult;
        }
    }
}
