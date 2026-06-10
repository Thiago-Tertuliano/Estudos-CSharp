# Aula 8 - Code Generation e Source Generators

## Objetivos da Aula
- Entender Source Generators
- Aprender Code Generation em tempo de compilação
- Compreender metaprogramação
- Praticar geração automática de código

## Conteúdo Teórico

### Source Generators Fundamentals

#### What are Source Generators?
Source Generators são componentes que executam durante a compilação e geram código C# adicional que é compilado junto com o resto do código.

```csharp
// Exemplo básico de Source Generator
[Generator]
public class HelloWorldGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        // Inicialização do generator
    }
    
    public void Execute(GeneratorExecutionContext context)
    {
        // Gerar código
        var sourceBuilder = new StringBuilder(@"
using System;

namespace Generated
{
    public static class HelloWorld
    {
        public static void SayHello()
        {
            Console.WriteLine(""Hello from generated code!"");
        }
    }
}");
        
        context.AddSource("HelloWorld.g.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
    }
}
```

### Creating Source Generators

#### Basic Source Generator
```csharp
[Generator]
public class BasicGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        // Registrar callbacks se necessário
    }
    
    public void Execute(GeneratorExecutionContext context)
    {
        // Gerar código baseado no contexto de compilação
        var sourceCode = @"
using System;

namespace Generated
{
    public static class GeneratedClass
    {
        public static string GetGeneratedString()
        {
            return ""This was generated at compile time!"";
        }
    }
}";
        
        context.AddSource("GeneratedClass.g.cs", SourceText.From(sourceCode, Encoding.UTF8));
    }
}
```

#### Attribute-based Generator
```csharp
// Attribute para marcar classes
[AttributeUsage(AttributeTargets.Class)]
public class GenerateToStringAttribute : Attribute
{
}

// Source Generator
[Generator]
public class ToStringGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        // Registrar para o attribute
        context.RegisterForSyntaxNotifications(() => new ToStringSyntaxReceiver());
    }
    
    public void Execute(GeneratorExecutionContext context)
    {
        var receiver = (ToStringSyntaxReceiver)context.SyntaxReceiver;
        
        foreach (var classDeclaration in receiver.CandidateClasses)
        {
            var sourceCode = GenerateToStringMethod(classDeclaration);
            context.AddSource($"{classDeclaration.Identifier.Text}.g.cs", 
                SourceText.From(sourceCode, Encoding.UTF8));
        }
    }
    
    private string GenerateToStringMethod(ClassDeclarationSyntax classDeclaration)
    {
        var className = classDeclaration.Identifier.Text;
        var properties = GetPublicProperties(classDeclaration);
        
        var toStringMethod = new StringBuilder();
        toStringMethod.AppendLine($"public override string ToString()");
        toStringMethod.AppendLine("{");
        toStringMethod.AppendLine($"    return $\"{className} {{ ");
        
        for (int i = 0; i < properties.Count; i++)
        {
            var property = properties[i];
            toStringMethod.AppendLine($"        {property.Name} = {{{property.Name}}}");
            if (i < properties.Count - 1)
                toStringMethod.AppendLine("        , ");
        }
        
        toStringMethod.AppendLine("    }}\";");
        toStringMethod.AppendLine("}");
        
        return toStringMethod.ToString();
    }
}

// Syntax Receiver para encontrar classes marcadas
public class ToStringSyntaxReceiver : ISyntaxReceiver
{
    public List<ClassDeclarationSyntax> CandidateClasses { get; } = new();
    
    public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
    {
        if (syntaxNode is ClassDeclarationSyntax classDeclaration)
        {
            if (classDeclaration.AttributeLists.Any(attr => 
                attr.Attributes.Any(a => a.Name.ToString() == "GenerateToString")))
            {
                CandidateClasses.Add(classDeclaration);
            }
        }
    }
}
```

### Advanced Source Generators

#### Interface Generator
```csharp
[Generator]
public class InterfaceGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new InterfaceSyntaxReceiver());
    }
    
    public void Execute(GeneratorExecutionContext context)
    {
        var receiver = (InterfaceSyntaxReceiver)context.SyntaxReceiver;
        
        foreach (var classDeclaration in receiver.CandidateClasses)
        {
            var interfaceCode = GenerateInterface(classDeclaration);
            var interfaceName = $"I{classDeclaration.Identifier.Text}";
            
            context.AddSource($"{interfaceName}.g.cs", 
                SourceText.From(interfaceCode, Encoding.UTF8));
        }
    }
    
    private string GenerateInterface(ClassDeclarationSyntax classDeclaration)
    {
        var className = classDeclaration.Identifier.Text;
        var interfaceName = $"I{className}";
        var properties = GetPublicProperties(classDeclaration);
        var methods = GetPublicMethods(classDeclaration);
        
        var interfaceCode = new StringBuilder();
        interfaceCode.AppendLine($"public interface {interfaceName}");
        interfaceCode.AppendLine("{");
        
        // Gerar propriedades
        foreach (var property in properties)
        {
            interfaceCode.AppendLine($"    {property.Type} {property.Name} {{ get; set; }}");
        }
        
        // Gerar métodos
        foreach (var method in methods)
        {
            interfaceCode.AppendLine($"    {method.ReturnType} {method.Name}({method.Parameters});");
        }
        
        interfaceCode.AppendLine("}");
        
        return interfaceCode.ToString();
    }
}
```

#### Builder Pattern Generator
```csharp
[AttributeUsage(AttributeTargets.Class)]
public class GenerateBuilderAttribute : Attribute
{
}

[Generator]
public class BuilderGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new BuilderSyntaxReceiver());
    }
    
    public void Execute(GeneratorExecutionContext context)
    {
        var receiver = (BuilderSyntaxReceiver)context.SyntaxReceiver;
        
        foreach (var classDeclaration in receiver.CandidateClasses)
        {
            var builderCode = GenerateBuilder(classDeclaration);
            var builderName = $"{classDeclaration.Identifier.Text}Builder";
            
            context.AddSource($"{builderName}.g.cs", 
                SourceText.From(builderCode, Encoding.UTF8));
        }
    }
    
    private string GenerateBuilder(ClassDeclarationSyntax classDeclaration)
    {
        var className = classDeclaration.Identifier.Text;
        var properties = GetPublicProperties(classDeclaration);
        
        var builderCode = new StringBuilder();
        builderCode.AppendLine($"public class {className}Builder");
        builderCode.AppendLine("{");
        
        // Campos privados
        foreach (var property in properties)
        {
            builderCode.AppendLine($"    private {property.Type} _{property.Name.ToLower()};");
        }
        
        // Métodos With
        foreach (var property in properties)
        {
            builderCode.AppendLine($"    public {className}Builder With{property.Name}({property.Type} {property.Name.ToLower()})");
            builderCode.AppendLine("    {");
            builderCode.AppendLine($"        _{property.Name.ToLower()} = {property.Name.ToLower()};");
            builderCode.AppendLine("        return this;");
            builderCode.AppendLine("    }");
        }
        
        // Método Build
        builderCode.AppendLine($"    public {className} Build()");
        builderCode.AppendLine("    {");
        builderCode.AppendLine($"        return new {className}");
        builderCode.AppendLine("        {");
        
        for (int i = 0; i < properties.Count; i++)
        {
            var property = properties[i];
            builderCode.AppendLine($"            {property.Name} = _{property.Name.ToLower()}");
            if (i < properties.Count - 1)
                builderCode.AppendLine(",");
        }
        
        builderCode.AppendLine("        };");
        builderCode.AppendLine("    }");
        builderCode.AppendLine("}");
        
        return builderCode.ToString();
    }
}
```

### Code Generation with Reflection

#### Runtime Code Generation
```csharp
public class DynamicTypeBuilder
{
    public static Type CreateType(string typeName, Dictionary<string, Type> properties)
    {
        var assemblyName = new AssemblyName("DynamicAssembly");
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        var moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");
        var typeBuilder = moduleBuilder.DefineType(typeName, TypeAttributes.Public | TypeAttributes.Class);
        
        // Criar propriedades
        foreach (var property in properties)
        {
            var fieldBuilder = typeBuilder.DefineField($"_{property.Key.ToLower()}", 
                property.Value, FieldAttributes.Private);
            
            var propertyBuilder = typeBuilder.DefineProperty(property.Key, 
                PropertyAttributes.HasDefault, property.Value, null);
            
            // Getter
            var getMethodBuilder = typeBuilder.DefineMethod($"get_{property.Key}",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                property.Value, Type.EmptyTypes);
            
            var getIL = getMethodBuilder.GetILGenerator();
            getIL.Emit(OpCodes.Ldarg_0);
            getIL.Emit(OpCodes.Ldfld, fieldBuilder);
            getIL.Emit(OpCodes.Ret);
            
            // Setter
            var setMethodBuilder = typeBuilder.DefineMethod($"set_{property.Key}",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig,
                null, new Type[] { property.Value });
            
            var setIL = setMethodBuilder.GetILGenerator();
            setIL.Emit(OpCodes.Ldarg_0);
            setIL.Emit(OpCodes.Ldarg_1);
            setIL.Emit(OpCodes.Stfld, fieldBuilder);
            setIL.Emit(OpCodes.Ret);
            
            propertyBuilder.SetGetMethod(getMethodBuilder);
            propertyBuilder.SetSetMethod(setMethodBuilder);
        }
        
        return typeBuilder.CreateType();
    }
}

// Uso
var properties = new Dictionary<string, Type>
{
    { "Name", typeof(string) },
    { "Age", typeof(int) },
    { "Email", typeof(string) }
};

var dynamicType = DynamicTypeBuilder.CreateType("DynamicPerson", properties);
var instance = Activator.CreateInstance(dynamicType);
```

### Expression Trees

#### Dynamic Expression Generation
```csharp
public class ExpressionGenerator
{
    public static Func<T, bool> CreatePredicate<T>(string propertyName, object value)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyName);
        var constant = Expression.Constant(value);
        var comparison = Expression.Equal(property, constant);
        
        return Expression.Lambda<Func<T, bool>>(comparison, parameter).Compile();
    }
    
    public static Func<T, TResult> CreateSelector<T, TResult>(string propertyName)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyName);
        
        return Expression.Lambda<Func<T, TResult>>(property, parameter).Compile();
    }
    
    public static Action<T> CreateSetter<T>(string propertyName, object value)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, propertyName);
        var constant = Expression.Constant(value);
        var assignment = Expression.Assign(property, constant);
        
        return Expression.Lambda<Action<T>>(assignment, parameter).Compile();
    }
}

// Uso
var predicate = ExpressionGenerator.CreatePredicate<Person>("Age", 25);
var selector = ExpressionGenerator.CreateSelector<Person, string>("Name");
var setter = ExpressionGenerator.CreateSetter<Person>("Age", 30);
```

### Code Generation for Performance

#### High-Performance Serialization
```csharp
[Generator]
public class SerializationGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new SerializationSyntaxReceiver());
    }
    
    public void Execute(GeneratorExecutionContext context)
    {
        var receiver = (SerializationSyntaxReceiver)context.SyntaxReceiver;
        
        foreach (var classDeclaration in receiver.CandidateClasses)
        {
            var serializerCode = GenerateSerializer(classDeclaration);
            var serializerName = $"{classDeclaration.Identifier.Text}Serializer";
            
            context.AddSource($"{serializerName}.g.cs", 
                SourceText.From(serializerCode, Encoding.UTF8));
        }
    }
    
    private string GenerateSerializer(ClassDeclarationSyntax classDeclaration)
    {
        var className = classDeclaration.Identifier.Text;
        var properties = GetPublicProperties(classDeclaration);
        
        var serializerCode = new StringBuilder();
        serializerCode.AppendLine($"public static class {className}Serializer");
        serializerCode.AppendLine("{");
        
        // Serialize method
        serializerCode.AppendLine($"    public static string Serialize({className} obj)");
        serializerCode.AppendLine("    {");
        serializerCode.AppendLine("        var sb = new StringBuilder();");
        serializerCode.AppendLine("        sb.Append(\"{\");");
        
        for (int i = 0; i < properties.Count; i++)
        {
            var property = properties[i];
            serializerCode.AppendLine($"        sb.Append($\"\\\"{property.Name}\\\":\\\"{{obj.{property.Name}}}\\\"\");");
            if (i < properties.Count - 1)
                serializerCode.AppendLine("        sb.Append(\",\");");
        }
        
        serializerCode.AppendLine("        sb.Append(\"}\");");
        serializerCode.AppendLine("        return sb.ToString();");
        serializerCode.AppendLine("    }");
        
        // Deserialize method
        serializerCode.AppendLine($"    public static {className} Deserialize(string json)");
        serializerCode.AppendLine("    {");
        serializerCode.AppendLine($"        var obj = new {className}();");
        serializerCode.AppendLine("        // Implementar deserialização");
        serializerCode.AppendLine("        return obj;");
        serializerCode.AppendLine("    }");
        
        serializerCode.AppendLine("}");
        
        return serializerCode.ToString();
    }
}
```

### Metaprogramming Patterns

#### Code Generation for Validation
```csharp
[AttributeUsage(AttributeTargets.Property)]
public class RequiredAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property)]
public class MaxLengthAttribute : Attribute
{
    public int Length { get; }
    public MaxLengthAttribute(int length) => Length = length;
}

[Generator]
public class ValidationGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new ValidationSyntaxReceiver());
    }
    
    public void Execute(GeneratorExecutionContext context)
    {
        var receiver = (ValidationSyntaxReceiver)context.SyntaxReceiver;
        
        foreach (var classDeclaration in receiver.CandidateClasses)
        {
            var validatorCode = GenerateValidator(classDeclaration);
            var validatorName = $"{classDeclaration.Identifier.Text}Validator";
            
            context.AddSource($"{validatorName}.g.cs", 
                SourceText.From(validatorCode, Encoding.UTF8));
        }
    }
    
    private string GenerateValidator(ClassDeclarationSyntax classDeclaration)
    {
        var className = classDeclaration.Identifier.Text;
        var properties = GetPropertiesWithAttributes(classDeclaration);
        
        var validatorCode = new StringBuilder();
        validatorCode.AppendLine($"public static class {className}Validator");
        validatorCode.AppendLine("{");
        validatorCode.AppendLine($"    public static ValidationResult Validate({className} obj)");
        validatorCode.AppendLine("    {");
        validatorCode.AppendLine("        var errors = new List<string>();");
        
        foreach (var property in properties)
        {
            if (property.HasRequired)
            {
                validatorCode.AppendLine($"        if (string.IsNullOrEmpty(obj.{property.Name}))");
                validatorCode.AppendLine($"            errors.Add(\"{property.Name} is required\");");
            }
            
            if (property.MaxLength.HasValue)
            {
                validatorCode.AppendLine($"        if (obj.{property.Name}?.Length > {property.MaxLength})");
                validatorCode.AppendLine($"            errors.Add(\"{property.Name} exceeds maximum length of {property.MaxLength}\");");
            }
        }
        
        validatorCode.AppendLine("        return new ValidationResult { IsValid = !errors.Any(), Errors = errors };");
        validatorCode.AppendLine("    }");
        validatorCode.AppendLine("}");
        
        return validatorCode.ToString();
    }
}
```

### Advanced Patterns

#### Code Generation for API Controllers
```csharp
[AttributeUsage(AttributeTargets.Class)]
public class GenerateControllerAttribute : Attribute
{
    public string Route { get; set; }
    public GenerateControllerAttribute(string route) => Route = route;
}

[Generator]
public class ControllerGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        context.RegisterForSyntaxNotifications(() => new ControllerSyntaxReceiver());
    }
    
    public void Execute(GeneratorExecutionContext context)
    {
        var receiver = (ControllerSyntaxReceiver)context.SyntaxReceiver;
        
        foreach (var classDeclaration in receiver.CandidateClasses)
        {
            var controllerCode = GenerateController(classDeclaration);
            var controllerName = $"{classDeclaration.Identifier.Text}Controller";
            
            context.AddSource($"{controllerName}.g.cs", 
                SourceText.From(controllerCode, Encoding.UTF8));
        }
    }
    
    private string GenerateController(ClassDeclarationSyntax classDeclaration)
    {
        var className = classDeclaration.Identifier.Text;
        var route = GetRouteFromAttribute(classDeclaration);
        
        var controllerCode = new StringBuilder();
        controllerCode.AppendLine("using Microsoft.AspNetCore.Mvc;");
        controllerCode.AppendLine("using System.Threading.Tasks;");
        controllerCode.AppendLine();
        controllerCode.AppendLine($"[ApiController]");
        controllerCode.AppendLine($"[Route(\"{route}\")]");
        controllerCode.AppendLine($"public class {className}Controller : ControllerBase");
        controllerCode.AppendLine("{");
        controllerCode.AppendLine($"    private readonly I{className}Service _service;");
        controllerCode.AppendLine();
        controllerCode.AppendLine($"    public {className}Controller(I{className}Service service)");
        controllerCode.AppendLine("    {");
        controllerCode.AppendLine("        _service = service;");
        controllerCode.AppendLine("    }");
        controllerCode.AppendLine();
        
        // GET all
        controllerCode.AppendLine("    [HttpGet]");
        controllerCode.AppendLine($"    public async Task<ActionResult<IEnumerable<{className}>>> GetAll()");
        controllerCode.AppendLine("    {");
        controllerCode.AppendLine($"        var items = await _service.GetAllAsync();");
        controllerCode.AppendLine("        return Ok(items);");
        controllerCode.AppendLine("    }");
        controllerCode.AppendLine();
        
        // GET by id
        controllerCode.AppendLine("    [HttpGet(\"{id}\")]");
        controllerCode.AppendLine($"    public async Task<ActionResult<{className}>> GetById(int id)");
        controllerCode.AppendLine("    {");
        controllerCode.AppendLine($"        var item = await _service.GetByIdAsync(id);");
        controllerCode.AppendLine("        if (item == null) return NotFound();");
        controllerCode.AppendLine("        return Ok(item);");
        controllerCode.AppendLine("    }");
        controllerCode.AppendLine();
        
        // POST
        controllerCode.AppendLine("    [HttpPost]");
        controllerCode.AppendLine($"    public async Task<ActionResult<{className}>> Create({className} item)");
        controllerCode.AppendLine("    {");
        controllerCode.AppendLine($"        var created = await _service.CreateAsync(item);");
        controllerCode.AppendLine("        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);");
        controllerCode.AppendLine("    }");
        controllerCode.AppendLine();
        
        // PUT
        controllerCode.AppendLine("    [HttpPut(\"{id}\")]");
        controllerCode.AppendLine($"    public async Task<IActionResult> Update(int id, {className} item)");
        controllerCode.AppendLine("    {");
        controllerCode.AppendLine("        if (id != item.Id) return BadRequest();");
        controllerCode.AppendLine($"        await _service.UpdateAsync(item);");
        controllerCode.AppendLine("        return NoContent();");
        controllerCode.AppendLine("    }");
        controllerCode.AppendLine();
        
        // DELETE
        controllerCode.AppendLine("    [HttpDelete(\"{id}\")]");
        controllerCode.AppendLine("    public async Task<IActionResult> Delete(int id)");
        controllerCode.AppendLine("    {");
        controllerCode.AppendLine($"        await _service.DeleteAsync(id);");
        controllerCode.AppendLine("        return NoContent();");
        controllerCode.AppendLine("    }");
        controllerCode.AppendLine("}");
        
        return controllerCode.ToString();
    }
}
```

## Exercícios

### Exercício 1 - Basic Source Generator
Crie um Source Generator que gera métodos de extensão para classes marcadas.

### Exercício 2 - Validation Generator
Implemente um generator que cria validadores baseados em attributes.

### Exercício 3 - API Generator
Crie um generator que gera controllers completos para entidades.

## Dicas
- Use Source Generators para código repetitivo
- Considere performance ao gerar código
- Documente generators complexos
- Teste generators com diferentes inputs
- Use reflection com cuidado
- Implemente error handling em generators
- Considere incremental generators para performance
- Mantenha código gerado simples e legível 