using System;

public class Criatura
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Especie Especie { get; private set; }
    public Medio Medio { get; private set; }
    public TipoAlimentacion Alimentacion { get; private set; }

    public int Edad { get; private set; }
    public int EdadAdulta { get; private set; }
    public int Salud { get; private set; } = 100;
    public int VecesFavorita { get; private set; }

    private int ApetitoBase;

    public Criatura(Especie especie)
    {
        Especie = especie;

        switch (especie)
        {
            case Especie.Gusano:
                Medio = Medio.Subterraneo;
                Alimentacion = TipoAlimentacion.Depredador;
                EdadAdulta = 24;
                ApetitoBase = 5;
                break;

            case Especie.TigreLaza:
                Medio = Medio.Desierto;
                Alimentacion = TipoAlimentacion.Depredador;
                EdadAdulta = 38;
                ApetitoBase = 8;
                break;

            case Especie.MuadDib:
                Medio = Medio.Desierto;
                Alimentacion = TipoAlimentacion.Recolector;
                EdadAdulta = 12;
                ApetitoBase = 2;
                break;

            case Especie.Halcon:
                Medio = Medio.Aereo;
                Alimentacion = TipoAlimentacion.Depredador;
                EdadAdulta = 16;
                ApetitoBase = 2;
                break;

            case Especie.Trucha:
                Medio = Medio.Subterraneo;
                Alimentacion = TipoAlimentacion.Recolector;
                EdadAdulta = 42;
                ApetitoBase = 10;
                break;
        }
    }

    public int CalcularIngesta(bool esAclimatacion)
    {
        if (Edad < EdadAdulta)
            return ApetitoBase * Edad;

        int alpha = esAclimatacion ? 15 : 1;
        return (int)(ApetitoBase * Math.Pow(2, Edad - EdadAdulta) * alpha);
    }

    public void Alimentar(int cantidad, bool esAclimatacion)
    {
        int necesaria = CalcularIngesta(esAclimatacion);
        double porcentaje = (double)cantidad / necesaria;

        if (porcentaje < 0.25) Salud -= 30;
        else if (porcentaje < 0.75) Salud -= 20;
        else if (porcentaje < 1) Salud -= 10;
        else if (Salud < 100) Salud += 5;

        if (Salud < 0) Salud = 0;
    }

    public void Envejecer()
    {
        Edad++;
    }

    public bool EsAdulta() => Edad >= EdadAdulta;

    public void MarcarFavorita()
    {
        VecesFavorita++;
    }

}