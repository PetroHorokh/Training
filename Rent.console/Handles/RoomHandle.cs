using Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Rent.BLL.Services;
using Rent.BLL.Services.Contracts;
using Rent.DAL.DTO;
using Rent.DAL.Models;

namespace Rent.console.Handles;

public static class RoomHandle
{
    public delegate Task RoomHandleDelegate();
    public static List<RoomHandleDelegate> RoomMenu { get; set; }

    private static readonly IRoomService RoomService;

    static RoomHandle()
    {
        RoomService = Program.Services.GetRequiredService<IRoomService>();
        RoomMenu = 
        [
            GetAllRooms, 
            GetAllRoomTypes, 
            GetAllAccommodations, 
            GetRoomByRoomId, 
            GetRoomByNumber,
            GetAccommodationsOfRoom,
            CreateRoom,
            CreateRoomType,
            CreateAccommodation,
            AddAccommodationRoom,
            UpdateAccommodationRoom,
            DeleteRoom,
            DeleteAccommodationRoom,
            () => Task.Run(() => MenuHandle.MainMenuSelector = 0),
        ];
    }

    private static async Task GetAllRooms()
    {
        var response = await RoomService.GetAllRoomsAsync();

        if (!response.Exceptions.IsNullOrEmpty())
        {
            Console.WriteLine("Error occured");
            return;
        }

        if (response.Body.IsNullOrEmpty()) Console.WriteLine("\nThere are no rooms");
        else
        {
            foreach (var room in response.Body!)
            {
                Console.WriteLine(room);
            }
        }
    }

    private static async Task GetAllRoomTypes()
    {
        var response = await RoomService.GetAllRoomTypesAsync();

        if (!response.Exceptions.IsNullOrEmpty())
        {
            Console.WriteLine("Error occured");
            return;
        }

        if (response.Body.IsNullOrEmpty()) Console.WriteLine("\nThere are no room types");
        else
        {
            foreach (var roomType in response.Body!)
            {
                Console.WriteLine(roomType);
            }
        }
    }

    private static async Task GetAllAccommodations()
    {
        var response = await RoomService.GetAllAccommodationsAsync();

        if (!response.Exceptions.IsNullOrEmpty())
        {
            Console.WriteLine("Error occured");
            return;
        }

        if (response.Body.IsNullOrEmpty()) Console.WriteLine("\nThere are no accommodations");
        else
        {
            foreach (var accommodation in response.Body!)
            {
                Console.WriteLine(accommodation);
            }
        }
    }

    private static async Task GetRoomByRoomId()
    {
        Console.Write("\nPlease enter required room id: ");
        string input = Console.ReadLine()!;

        if (Guid.TryParse(input, out Guid roomId))
        {
            var response = await RoomService.GetRoomByRoomIdAsync(roomId);

            if (!response.Exceptions.IsNullOrEmpty())
            {
                Console.WriteLine("Error occured");
                return;
            }

            Console.WriteLine(response.Body is not null ?
                response.Body :
                "\nThere is no such room");
        }
        else
        {
            Console.WriteLine("\nWrong id format");
        }
    }

    private static async Task GetRoomByNumber()
    {
        Console.Write("\nPlease enter required room number: ");
        string input = Console.ReadLine()!;

        if (int.TryParse(input, out int roomNumber))
        {
            var response = await RoomService.GetRoomByNumberAsync(roomNumber);

            if (!response.Exceptions.IsNullOrEmpty())
            {
                Console.WriteLine("Error occured");
                return;
            }

            Console.WriteLine(response.Body is not null ?
                response.Body :
                "\nThere is no such room");
        }
        else
        {
            Console.WriteLine("\nWrong number format");
        }
    }

    private static async Task GetAccommodationsOfRoom()
    {
        Console.Write("\nPlease enter required room id: ");
        string input = Console.ReadLine()!;

        if (Guid.TryParse(input, out Guid roomId))
        {
            var response = await RoomService.GetAccommodationRoomsByRoomIdAsync(roomId);

            if (!response.Exceptions.IsNullOrEmpty())
            {
                Console.WriteLine("Error occured");
                return;
            }

            if (response.Body.IsNullOrEmpty()) Console.WriteLine("\nThere are no accommodations for a room");
            else
            {
                foreach (var accommodation in response.Body!)
                {
                    Console.WriteLine(accommodation);
                }
            }
        }
        else
        {
            Console.WriteLine("\nWrong id format");
        }
    }

    private static async Task CreateRoom()
    {
        Console.WriteLine("\nCreation of new room");

        int number, roomTypeId;
        double area;
        string input;

        do
        {
            Console.Write("\nEnter room's number: ");
            input = Console.ReadLine()!;
        } while (!int.TryParse(input, out number));

        do
        {
            Console.Write("\nEnter room's area: ");
            input = Console.ReadLine()!;
        } while (!double.TryParse(input, out area));

        do
        {
            Console.Write("\nEnter room's type: ");
            input = Console.ReadLine()!;
        } while (!int.TryParse(input, out roomTypeId));

        var room = new RoomToCreateDto()
        {
            Number = number,
            Area = Convert.ToDecimal(area),
            RoomTypeId = roomTypeId
        };

        var response = await RoomService.CreateRoomAsync(room);

        Console.WriteLine(response.Exceptions.IsNullOrEmpty()! ? "\nError occured" : $"\nSuccessfully created a new room with id {response.Body}");
    }

    private static async Task CreateAccommodation()
    {
        string name;
        do
        {
            Console.Write("\nPlease enter accommodation name: ");
            name = Console.ReadLine()!;
        } while (name.Length == 0);


        var accommodation = new AccommodationToCreateDto()
        {
            Name = name,
        };

        var result = await RoomService.CreateAccommodationAsync(accommodation);

        Console.WriteLine(result.Exceptions.IsNullOrEmpty()! ? '\n' + "Error occured" : $"\nSuccessfully added a accommodation with id {result.Body}");
    }

    private static async Task CreateRoomType()
    {
        string name;
        do
        {
            Console.Write("\nPlease enter room type name: ");
            name = Console.ReadLine()!;
        } while (name.Length == 0);


        var roomType = new RoomTypeToCreateDto()
        {
            Name = name,
        };

        var result = await RoomService.CreateRoomTypeAsync(roomType);

        Console.WriteLine(result.Exceptions.IsNullOrEmpty()! ? "\nError occured" : $"\nSuccessfully added a accommodation with id {result.Body}");
    }

    private static async Task AddAccommodationRoom()
    {
        Guid roomId;
        int quantity, accommodationId;
        string input;

        do
        {
            Console.Write("\nPlease enter required room id for adding new accommodation: ");
            input = Console.ReadLine()!;
        } while (!Guid.TryParse(input, out roomId));

        do
        {
            Console.Write("\nPlease enter accommodation id: ");
            input = Console.ReadLine()!;
        } while (!int.TryParse(input, out accommodationId));

        do
        {
            Console.Write("\nPlease enter quantity: ");
            input = Console.ReadLine()!;
        } while (!int.TryParse(input, out quantity));

        var accommodationRoom = new AccommodationRoomToCreateDto()
        {
            AccommodationId = accommodationId,
            RoomId = roomId,
            Quantity = quantity
        };

        var result = await RoomService.CreateAccommodationRoomAsync(accommodationRoom);

        Console.WriteLine(result.Exceptions.IsNullOrEmpty()! ? "\nError occured" : $"\nSuccessfully added a accommodation to room with id {result.Body}");
    }

    private static async Task UpdateAccommodationRoom()
    {
        Console.Write("\nPlease enter required room accommodation quantity of which needed to be changed: ");
        string input = Console.ReadLine()!;

        if (Guid.TryParse(input, out Guid accommodationRoomId))
        {
            var response = await RoomService.GetAccommodationRoomByIdAsync(accommodationRoomId);

            if (response.Body is not null)
            {
                Console.WriteLine("\nRoom accommodation edit");

                Console.WriteLine($"\nOld accommodation quantity: {response.Body.Quantity}");

                do
                {
                    Console.Write($"\nWould you like to change accommodation quantity? (Y/N): ");
                    input = Console.ReadLine()!;
                } while (!EditDecision(input));


                if (EditConfirm(input))
                {
                    do
                    {
                        Console.Write($"\nNew accommodation quantity: ");
                        input = Console.ReadLine()!;
                        if (int.TryParse(input, out int quantity))
                        {
                            response.Body.Quantity = quantity;
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"\nWrong number format");
                        }

                    } while (true);
                }

                var accommodationRoomToUpdateDto = new AccommodationRoomToGetDto()
                {
                    AccommodationRoomId = response.Body.AccommodationRoomId,
                    Quantity = response.Body.Quantity,
                    
                };

                var result = await RoomService.UpdateAccommodationRoom(accommodationRoomToUpdateDto);

                Console.WriteLine(result.Exceptions.IsNullOrEmpty()! ? "\nError occured" : "Changes were saved");
            }
            else
            {
                Console.WriteLine("\nThere is no such accommodation");
            }
        }
        else
        {
            Console.WriteLine("\nWrong id format");
        }
    }

    private static async Task DeleteRoom()
    {
        Console.Write("\nPlease enter required room id for deletion: ");
        string input = Console.ReadLine()!;

        if (Guid.TryParse(input, out var roomId))
        {
            var result = await RoomService.DeleteRoomAsync(roomId);

            Console.WriteLine(result.Exceptions.IsNullOrEmpty()! ? "\nError occured" : "\nSuccessfully deleted a room");
        }
        else
        {
            Console.WriteLine("\nWrong id format");
        }
    }

    private static async Task DeleteAccommodationRoom()
    {
        Console.Write("\nPlease enter required room accommodation id for deletion: ");
        string input = Console.ReadLine()!;

        if (Guid.TryParse(input, out var accommodationRoomId))
        {
            var result = await RoomService.DeleteAccommodationRoomAsync(accommodationRoomId);

            Console.WriteLine(result.Exceptions.IsNullOrEmpty()! ? "\nError occured" : "\nSuccessfully deleted a room");
        }
        else
        {
            Console.WriteLine("\nWrong id format");
        }
    }

    private static bool EditDecision(string input) =>
        string.CompareOrdinal(input.ToLower(), "y") == 0 || string.CompareOrdinal(input.ToLower(), "n") == 0 ||
        string.CompareOrdinal(input.ToLower(), "yes") == 0 || string.CompareOrdinal(input.ToLower(), "no") == 0;

    private static bool EditConfirm(string input) => string.CompareOrdinal(input.ToLower(), "y") == 0 ||
                                                     string.CompareOrdinal(input.ToLower(), "yes") == 0;
}