namespace PlutoNetCoreTemplate.Job.Hosting.Infrastructure
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;


    /// <summary>
    /// 定长队列
    /// </summary>
    public class FixLengthQueue:Queue
    {
        private readonly int _length;
        public FixLengthQueue(int length)
        {
            this._length = length;
        }

        /// <summary>
        /// 默认长度10
        /// </summary>
        public FixLengthQueue() : this(10)
        {
        }


        /// <inheritdoc />
        public override void Enqueue(object obj)
        {
            if (this.Count>=_length)
            {
                this.Dequeue();
            }
            base.Enqueue(obj);
        }

    }

}