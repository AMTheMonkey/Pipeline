using System;

namespace PipedMediator
{
    public class PipelineBuilder
    {
        private readonly IServiceProvider _serviceProvider;

        public PipelineBuilder(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public static PipelineBuilder<TInput> AddPipeline<TPipeline, TInput>()
            where TPipeline : IPipeline<TInput>, new()
        {
            return new PipelineBuilder<TInput>(new Pipeline<TInput>(new TPipeline()));
        }

        public PipelineBuilder<TInput> AddPipeline<TPipeline, TInput>(
            Func<IServiceProvider, TPipeline> factory
        )
            where TPipeline : IPipeline<TInput>
        {
            return new PipelineBuilder<TInput>(new Pipeline<TInput>(factory(_serviceProvider)));
        }

        public PipelineBuilder<TInput, TOutput> AddPipeline<TPipeline, TInput, TOutput>()
            where TPipeline : IPipeline<TInput, TOutput>, new()
        {
            return new PipelineBuilder<TInput, TOutput>(
                this._serviceProvider,
                new Pipeline<TInput, TOutput>(new TPipeline())
            );
        }

        public PipelineBuilder<TInput, TOutput> AddPipeline<TPipeline, TInput, TOutput>(
            Func<IServiceProvider, TPipeline> factory
        )
            where TPipeline : IPipeline<TInput, TOutput>
        {
            return new PipelineBuilder<TInput, TOutput>(
                this._serviceProvider,
                new Pipeline<TInput, TOutput>(factory(_serviceProvider))
            );
        }
    }

    public class PipelineBuilder<TInput>
    {
        private readonly Pipeline<TInput> _pipeline;

        public PipelineBuilder(Pipeline<TInput> pipeline)
        {
            _pipeline = pipeline;
        }

        public IPipeline<TInput> Create() => this._pipeline;
    }

    public class PipelineBuilder<TInput, TOutput>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Pipeline<TInput, TOutput> _pipeline;

        public PipelineBuilder(IServiceProvider serviceProvider, Pipeline<TInput, TOutput> pipeline)
        {
            _serviceProvider = serviceProvider;
            _pipeline = pipeline;
        }

        public PipelineBuilder<TInput> AddPipeline<TPipeline>()
            where TPipeline : IPipeline<TOutput>, new()
        {
            return new PipelineBuilder<TInput>(_pipeline.Pipe(new TPipeline()));
        }

        public PipelineBuilder<TInput> AddPipeline<TPipeline>(
            Func<IServiceProvider, TPipeline> factory
        )
            where TPipeline : IPipeline<TOutput>
        {
            return new PipelineBuilder<TInput>(_pipeline.Pipe(factory(_serviceProvider)));
        }

        public PipelineBuilder<TInput, TNextOutput> AddPipeline<TPipeline, TNextOutput>()
            where TPipeline : IPipeline<TOutput, TNextOutput>, new()
        {
            return new PipelineBuilder<TInput, TNextOutput>(
                this._serviceProvider,
                _pipeline.Pipe<TPipeline, TNextOutput>(new TPipeline())
            );
        }

        public PipelineBuilder<TInput, TNextOutput> AddPipeline<TPipeline, TNextOutput>(
            Func<IServiceProvider, TPipeline> factory
        )
            where TPipeline : IPipeline<TOutput, TNextOutput>
        {
            return new PipelineBuilder<TInput, TNextOutput>(
                this._serviceProvider,
                _pipeline.Pipe<TPipeline, TNextOutput>(factory(_serviceProvider))
            );
        }

        public IPipeline<TInput, TOutput> Create() => this._pipeline;
    }
}
