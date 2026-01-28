
namespace FrogLib;

public ref struct FastDeserializer {


    public int ReadCount => read;
    public int UnreadCount => unread;


    private ReadOnlySpan<byte> data;
    private int read;
    private int unread => data.Length - read;
    public FastDeserializer(ReadOnlySpan<byte> data) {
        this.data = data;
    }

    public unsafe FastDeserializer Read<T>(out T value) where T : unmanaged {

        if (unread < sizeof(T)) throw new IndexOutOfRangeException();

        fixed (byte* ptr = data) {
            value = *(T*)(ptr + read);
        }

        read += sizeof(T);

        return this;
    }

    public unsafe FastDeserializer Read<T>(out ReadOnlySpan<T> values, int count) where T : unmanaged {

        if (unread < sizeof(T) * count) throw new IndexOutOfRangeException();

        fixed (byte* ptr = data) {
            values = new ReadOnlySpan<T>(ptr + read, count);
        }

        read += sizeof(T) * count;
        return this;

    }


    public unsafe FastDeserializer Read(out string str) {

        if (unread < sizeof(int)) throw new IndexOutOfRangeException();

        fixed (byte* ptr = data) {

            var length = *(int*)(ptr + read);

            read += sizeof(int);

            if (length == 0) {
                str = string.Empty;
                return this;
            }

            int size = length * sizeof(char);

            if (unread < size) throw new IndexOutOfRangeException();

            str = new string((char*)(ptr + read), 0, length);

            read += size;
        }

        return this;
    }

    public FastDeserializer Skip(int size) {
        read += size;
        return this;
    }
}