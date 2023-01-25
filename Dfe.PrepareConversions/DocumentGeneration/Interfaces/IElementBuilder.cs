namespace DocumentGeneration.Interfaces
{
    public interface IElementBuilder<out T>
    {
        public T Build();
    }
}