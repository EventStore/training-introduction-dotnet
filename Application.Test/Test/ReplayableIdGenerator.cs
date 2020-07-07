using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Test.Test
{
    public class ReplayableIdGenerator
    {
        private bool _replayMode = false;
        private Queue<Guid> _ids = new Queue<Guid>();


        public Guid NextGuid()
        {
            if (_replayMode)
            {
                if (!_ids.Any())
                {
                    throw new Exception("Replayable Id generator doesn't have any ids left to replay");
                }

                var id = _ids.Dequeue();
                return id;
            }
            else
            {
                var id = Guid.NewGuid();
                _ids.Enqueue(id);
                return id;
            }
        }

        public void Replay()
        {
            _replayMode = true;
        }

        public void Reset()
        {
            _replayMode = false;
            _ids.Clear();
        }
    }
}