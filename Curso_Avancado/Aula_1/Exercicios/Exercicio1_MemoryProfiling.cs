using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace Aula1_Exercicios
{
    /// <summary>
    /// Exercício 1 - Memory Profiling
    /// 
    /// Objetivo: Demonstrar diferentes padrões de uso de memória e usar ferramentas de profiling.
    /// 
    /// Tarefas:
    /// 1. Implementar diferentes padrões de alocação de memória
    /// 2. Comparar performance entre Value Types e Reference Types
    /// 3. Demonstrar uso de Span<T> e Memory<T>
    /// 4. Implementar Object Pooling
    /// 5. Analisar uso de memória com profiling
    /// </summary>
    public class MemoryProfilingExercise
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("=== Exercício 1 - Memory Profiling ===\n");
            
            var exercise = new MemoryProfilingExercise();
            
            // 1. Comparar Value Types vs Reference Types
            Console.WriteLine("1. Comparando Value Types vs Reference Types:");
            exercise.CompareValueVsReferenceTypes();
            
            // 2. Demonstrar Boxing/Unboxing
            Console.WriteLine("\n2. Demonstrando Boxing/Unboxing:");
            exercise.DemonstrateBoxingUnboxing();
            
            // 3. Usar Span<T> para performance
            Console.WriteLine("\n3. Usando Span<T> para performance:");
            exercise.DemonstrateSpanUsage();
            
            // 4. Object Pooling
            Console.WriteLine("\n4. Implementando Object Pooling:");
            exercise.DemonstrateObjectPooling();
            
            // 5. Memory Leaks
            Console.WriteLine("\n5. Demonstrando Memory Leaks:");
            exercise.DemonstrateMemoryLeaks();
            
            // 6. Performance Benchmark
            Console.WriteLine("\n6. Benchmark de Performance:");
            await exercise.RunPerformanceBenchmark();
            
            Console.WriteLine("\n=== Exercício concluído! ===");
        }
        
        /// <summary>
        /// Compara performance entre Value Types e Reference Types
        /// </summary>
        public void CompareValueVsReferenceTypes()
        {
            const int iterations = 1000000;
            
            // Teste com Value Types (structs)
            var stopwatch = Stopwatch.StartNew();
            var valueList = new List<PointStruct>();
            
            for (int i = 0; i < iterations; i++)
            {
                valueList.Add(new PointStruct { X = i, Y = i * 2 });
            }
            
            stopwatch.Stop();
            var valueTypeTime = stopwatch.ElapsedMilliseconds;
            var valueTypeMemory = GC.GetTotalMemory(false);
            
            GC.Collect();
            
            // Teste com Reference Types (classes)
            stopwatch.Restart();
            var referenceList = new List<PointClass>();
            
            for (int i = 0; i < iterations; i++)
            {
                referenceList.Add(new PointClass { X = i, Y = i * 2 });
            }
            
            stopwatch.Stop();
            var referenceTypeTime = stopwatch.ElapsedMilliseconds;
            var referenceTypeMemory = GC.GetTotalMemory(false);
            
            Console.WriteLine($"Value Types: {valueTypeTime}ms, Memory: {valueTypeMemory / 1024}KB");
            Console.WriteLine($"Reference Types: {referenceTypeTime}ms, Memory: {referenceTypeMemory / 1024}KB");
            Console.WriteLine($"Diferença de tempo: {referenceTypeTime - valueTypeTime}ms");
            Console.WriteLine($"Diferença de memória: {(referenceTypeMemory - valueTypeMemory) / 1024}KB");
        }
        
        /// <summary>
        /// Demonstra o impacto do boxing/unboxing
        /// </summary>
        public void DemonstrateBoxingUnboxing()
        {
            const int iterations = 1000000;
            
            // Teste com boxing (ruim)
            var stopwatch = Stopwatch.StartNew();
            var boxedList = new List<object>();
            
            for (int i = 0; i < iterations; i++)
            {
                boxedList.Add(i); // Boxing ocorre aqui
            }
            
            stopwatch.Stop();
            var boxedTime = stopwatch.ElapsedMilliseconds;
            
            // Teste sem boxing (bom)
            stopwatch.Restart();
            var unboxedList = new List<int>();
            
            for (int i = 0; i < iterations; i++)
            {
                unboxedList.Add(i); // Sem boxing
            }
            
            stopwatch.Stop();
            var unboxedTime = stopwatch.ElapsedMilliseconds;
            
            Console.WriteLine($"Com Boxing: {boxedTime}ms");
            Console.WriteLine($"Sem Boxing: {unboxedTime}ms");
            Console.WriteLine($"Melhoria: {((double)(boxedTime - unboxedTime) / boxedTime * 100):F1}%");
        }
        
        /// <summary>
        /// Demonstra uso de Span<T> para performance
        /// </summary>
        public void DemonstrateSpanUsage()
        {
            var data = new byte[1000];
            new Random().NextBytes(data);
            
            // Método tradicional
            var stopwatch = Stopwatch.StartNew();
            var sum1 = SumBytesTraditional(data);
            stopwatch.Stop();
            var traditionalTime = stopwatch.ElapsedTicks;
            
            // Método com Span<T>
            stopwatch.Restart();
            var sum2 = SumBytesWithSpan(data);
            stopwatch.Stop();
            var spanTime = stopwatch.ElapsedTicks;
            
            Console.WriteLine($"Soma tradicional: {sum1}, Tempo: {traditionalTime} ticks");
            Console.WriteLine($"Soma com Span: {sum2}, Tempo: {spanTime} ticks");
            Console.WriteLine($"Melhoria: {((double)(traditionalTime - spanTime) / traditionalTime * 100):F1}%");
        }
        
        /// <summary>
        /// Demonstra Object Pooling
        /// </summary>
        public void DemonstrateObjectPooling()
        {
            const int iterations = 10000;
            
            // Sem Object Pooling
            var stopwatch = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                var obj = new ExpensiveObject();
                obj.DoWork();
                // Objeto é coletado pelo GC
            }
            stopwatch.Stop();
            var withoutPoolTime = stopwatch.ElapsedMilliseconds;
            
            // Com Object Pooling
            var pool = new ObjectPool<ExpensiveObject>(100);
            stopwatch.Restart();
            for (int i = 0; i < iterations; i++)
            {
                var obj = pool.Get();
                obj.DoWork();
                pool.Return(obj);
            }
            stopwatch.Stop();
            var withPoolTime = stopwatch.ElapsedMilliseconds;
            
            Console.WriteLine($"Sem Pool: {withoutPoolTime}ms");
            Console.WriteLine($"Com Pool: {withPoolTime}ms");
            Console.WriteLine($"Melhoria: {((double)(withoutPoolTime - withPoolTime) / withoutPoolTime * 100):F1}%");
        }
        
        /// <summary>
        /// Demonstra memory leaks
        /// </summary>
        public void DemonstrateMemoryLeaks()
        {
            Console.WriteLine("Criando objetos que podem causar memory leaks...");
            
            // Simular memory leak com eventos
            var leakyObjects = new List<LeakyObject>();
            for (int i = 0; i < 1000; i++)
            {
                leakyObjects.Add(new LeakyObject());
            }
            
            var memoryBefore = GC.GetTotalMemory(false);
            Console.WriteLine($"Memória antes: {memoryBefore / 1024}KB");
            
            // Limpar referências (mas eventos ainda mantêm objetos vivos)
            leakyObjects.Clear();
            GC.Collect();
            
            var memoryAfter = GC.GetTotalMemory(false);
            Console.WriteLine($"Memória depois: {memoryAfter / 1024}KB");
            Console.WriteLine($"Objetos não liberados: {(memoryAfter - memoryBefore) / 1024}KB");
            
            // Demonstrar solução com WeakReference
            Console.WriteLine("\nUsando WeakReference para evitar memory leaks:");
            var weakObjects = new List<WeakReference>();
            for (int i = 0; i < 1000; i++)
            {
                weakObjects.Add(new WeakReference(new SafeObject()));
            }
            
            memoryBefore = GC.GetTotalMemory(false);
            Console.WriteLine($"Memória antes: {memoryBefore / 1024}KB");
            
            GC.Collect();
            
            memoryAfter = GC.GetTotalMemory(false);
            Console.WriteLine($"Memória depois: {memoryAfter / 1024}KB");
            Console.WriteLine($"Objetos liberados: {(memoryBefore - memoryAfter) / 1024}KB");
        }
        
        /// <summary>
        /// Executa benchmark de performance
        /// </summary>
        public async Task RunPerformanceBenchmark()
        {
            Console.WriteLine("Executando benchmark de performance...");
            
            var results = new List<BenchmarkResult>();
            
            // Benchmark 1: String concatenation
            results.Add(await BenchmarkStringConcatenation());
            
            // Benchmark 2: StringBuilder
            results.Add(await BenchmarkStringBuilder());
            
            // Benchmark 3: Span<char>
            results.Add(await BenchmarkSpanChar());
            
            // Exibir resultados
            Console.WriteLine("\nResultados do Benchmark:");
            Console.WriteLine("Método\t\t\tTempo (ms)\tMemória (KB)");
            Console.WriteLine("-------\t\t\t---------\t------------");
            
            foreach (var result in results)
            {
                Console.WriteLine($"{result.Name,-20}\t{result.Time,-10}\t{result.Memory,-10}");
            }
        }
        
        // Métodos auxiliares
        private int SumBytesTraditional(byte[] data)
        {
            int sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                sum += data[i];
            }
            return sum;
        }
        
        private int SumBytesWithSpan(byte[] data)
        {
            Span<byte> span = data;
            int sum = 0;
            for (int i = 0; i < span.Length; i++)
            {
                sum += span[i];
            }
            return sum;
        }
        
        private async Task<BenchmarkResult> BenchmarkStringConcatenation()
        {
            var stopwatch = Stopwatch.StartNew();
            var memoryBefore = GC.GetTotalMemory(false);
            
            string result = "";
            for (int i = 0; i < 10000; i++)
            {
                result += $"Item {i}";
            }
            
            stopwatch.Stop();
            var memoryAfter = GC.GetTotalMemory(false);
            
            return new BenchmarkResult
            {
                Name = "String Concat",
                Time = stopwatch.ElapsedMilliseconds,
                Memory = (memoryAfter - memoryBefore) / 1024
            };
        }
        
        private async Task<BenchmarkResult> BenchmarkStringBuilder()
        {
            var stopwatch = Stopwatch.StartNew();
            var memoryBefore = GC.GetTotalMemory(false);
            
            var sb = new StringBuilder();
            for (int i = 0; i < 10000; i++)
            {
                sb.Append($"Item {i}");
            }
            var result = sb.ToString();
            
            stopwatch.Stop();
            var memoryAfter = GC.GetTotalMemory(false);
            
            return new BenchmarkResult
            {
                Name = "StringBuilder",
                Time = stopwatch.ElapsedMilliseconds,
                Memory = (memoryAfter - memoryBefore) / 1024
            };
        }
        
        private async Task<BenchmarkResult> BenchmarkSpanChar()
        {
            var stopwatch = Stopwatch.StartNew();
            var memoryBefore = GC.GetTotalMemory(false);
            
            var chars = new char[10000 * 8]; // Estimativa
            var span = chars.AsSpan();
            var position = 0;
            
            for (int i = 0; i < 10000; i++)
            {
                var item = $"Item {i}";
                item.AsSpan().CopyTo(span.Slice(position));
                position += item.Length;
            }
            
            stopwatch.Stop();
            var memoryAfter = GC.GetTotalMemory(false);
            
            return new BenchmarkResult
            {
                Name = "Span<char>",
                Time = stopwatch.ElapsedMilliseconds,
                Memory = (memoryAfter - memoryBefore) / 1024
            };
        }
    }
    
    // Classes auxiliares
    public struct PointStruct
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
    
    public class PointClass
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
    
    public class ExpensiveObject
    {
        private readonly byte[] _data = new byte[1024];
        
        public void DoWork()
        {
            // Simular trabalho custoso
            for (int i = 0; i < _data.Length; i++)
            {
                _data[i] = (byte)(i % 256);
            }
        }
    }
    
    public class ObjectPool<T> where T : class, new()
    {
        private readonly Queue<T> _pool = new();
        private readonly int _maxSize;
        
        public ObjectPool(int maxSize = 100)
        {
            _maxSize = maxSize;
        }
        
        public T Get()
        {
            if (_pool.TryDequeue(out T item))
            {
                return item;
            }
            return new T();
        }
        
        public void Return(T item)
        {
            if (item != null && _pool.Count < _maxSize)
            {
                _pool.Enqueue(item);
            }
        }
    }
    
    public class LeakyObject
    {
        public event EventHandler SomethingHappened;
        
        public LeakyObject()
        {
            // Simular subscription que não é removida
            SomethingHappened += OnSomethingHappened;
        }
        
        private void OnSomethingHappened(object sender, EventArgs e)
        {
            // Handler que mantém referência
        }
    }
    
    public class SafeObject
    {
        private WeakReference<EventHandler> _handler;
        
        public void Subscribe(EventHandler handler)
        {
            _handler = new WeakReference<EventHandler>(handler);
        }
    }
    
    public class BenchmarkResult
    {
        public string Name { get; set; }
        public long Time { get; set; }
        public long Memory { get; set; }
    }
} 