using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EdwardHsu.Lab.FileSliceSample
{
    public class StreamSlicer : IEnumerable<byte[]>, IEnumerator<byte[]>, IDisposable
    {
        // IEnumerator
        private int _offset = 0;
        private readonly Stream _stream;
        private byte[] _buffer;
        private int _usedBufferSize;
        private StreamSlicer self;
        object IEnumerator.Current => self.Current;

        public byte[] Current
        {
            get
            {
                return self._buffer.Take(self._usedBufferSize).ToArray();
            }
        }

        private StreamSlicer(StreamSlicer streamSlicer)
        {
            self = streamSlicer;
            _offset = self._offset;
            _stream = self._stream;
            _buffer = self._buffer;
            _usedBufferSize = self._usedBufferSize;
        }

        public StreamSlicer(Stream stream, int bufferSize = 1024 * 4)
        {
            _stream = stream;
            _buffer = new byte[bufferSize];
        }

        public bool MoveNext()
        {
            LoadSlice();
            _offset += _buffer.Length;
            return _usedBufferSize > 0;
        }

        public void Reset()
        {
            _offset = 0;
            LoadSlice();
        }

        private void LoadSlice()
        {
            _stream.Seek(_offset, SeekOrigin.Begin);
            _usedBufferSize = _stream.Read(_buffer, 0, _buffer.Length);
        }

        public void Dispose()
        {
            if (self == null)
            {
                _stream.Dispose();
            }
            else
            {
                _offset = 0;
            }
        }



        // IEnumerable
        public IEnumerator<byte[]> GetEnumerator()
        {
            return new StreamSlicer(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new StreamSlicer(this);
        }
    }
}
