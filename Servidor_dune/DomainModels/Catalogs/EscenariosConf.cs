using System;
using DomainModels.Catalogs.Escenario;

public static class EscenariosPredefinidos
{
    public static Escenario ArrakeenDominioDeLaEspecia = new Escenario(
        "Arrakeen: Dominio de la Especia",
        "Escenario situado en Arrakis, planeta desértico y única fuente natural de melange. Operación prestigiosa, compleja y altamente rentable.",
        100000,
        "Arrakeen"
    );

    public static Escenario GiediPrimeGaleriaIndustrial = new Escenario(
        "Giedi Prime: Galería Industrial",
        "Escenario de alta afluencia y baja exclusividad, marcado por una estética industrial y una operación compacta.",
        50000,
        "Giedi Prime"
    );

    public static Escenario CaladanReservaDucal = new Escenario(
        "Caladan: Reserva Ducal",
        "Escenario asociado al mundo oceánico históricamente vinculado a la Casa Atreides. Ofrece mejores condiciones logísticas y mayor inversión inicial.",
        150000,
        "Caladan"
    );
}