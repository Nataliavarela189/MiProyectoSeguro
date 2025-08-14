namespace WebApiPerson.Models
{
    public class Person
    {
        public int Id { get; set; }
        public required string Name { get; set; }               // Nombre completo
        public required string Identification { get; set; }     // Identificación (DNI, etc.)
        public required int Age { get; set; }                    // Edad
        public required string Gender { get; set; }             // Género
        public bool IsActive { get; set; }                       // Estado (Activo o no)

        // Atributos adicionales
        public bool Drives { get; set; }                         // ¿Maneja?
        public bool WearsGlasses { get; set; }                   // ¿Usa lentes?
        public bool HasDiabetes { get; set; }                    // ¿Diabético?
        public string? OtherDiseases { get; set; }               // ¿Padece alguna otra enfermedad? ¿Cuál?
    }
}

