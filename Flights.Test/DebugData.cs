using System.Globalization;
using CsvHelper;
using Flights.Domain.Entities;

namespace Flights.Test;

public record DebugDataDto(
    int Round,
    int Player,
    string Mod,
    int Value);

public class DebugData
{
    public async Task SaveData(GameEntity game, string filePath)
    {
        var dataDtos = new List<DebugDataDto>();
        
        foreach (var round in game.Rounds)
        foreach (var player in round.RoundStats)
        {
            if (player.FirstDart == null
                && player.SecondDart == null
                && player.ThirdDart == null)
                continue;
            
            dataDtos.Add(new DebugDataDto(
                round.Number,
                player.OrderNumber,
                GetModifierString(player.FirstDart!),
                player.FirstDart!.Value));

            if (player.SecondDart == null)
                continue;
            
            dataDtos.Add(new DebugDataDto(
                round.Number,
                player.OrderNumber,
                GetModifierString(player.SecondDart!),
                player.SecondDart.Value));
            
            if (player.ThirdDart == null)
                continue;
            
            dataDtos.Add(new DebugDataDto(
                round.Number,
                player.OrderNumber,
                GetModifierString(player.ThirdDart!),
                player.ThirdDart.Value));
        }

        await using var writer = new StreamWriter(filePath);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        await csv.WriteRecordsAsync(dataDtos);
    }

    private string GetModifierString(DartStatEntity dart)
    {
        switch (dart.Modifier)
        {
            case DartModifier.None:
                return "N";
            case DartModifier.Double:
                return "D";
            case DartModifier.Triple:
                return "T";
        }

        return "?";
    }

    public async Task<List<DebugDataDto>> LoadData(string filePath)
    {
        var result = new List<DebugDataDto>();
        
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecordsAsync<DebugDataDto>();
        await foreach (var rec in records)
            result.Add(rec);

        return result;
    }
}