using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallTypesHelper
{ 
    public static HashSet<int> wallNorth = new HashSet<int>
    {
        0b0010,
        /*
        0b1111,
        0b0110,
        0b0011,
        0b0010,
        0b1010,
        0b1100,
        0b1110,
        0b1011,
        0b0111,
        */
    };

    public static HashSet<int> wallWest = new HashSet<int>
    {
        0b0100
    };

    public static HashSet<int> wallEast = new HashSet<int>
    {
        0b0001
    };

    public static HashSet<int> wallSouth = new HashSet<int>
    {
        0b1000
    };

    public static HashSet<int> wallInnerCornerNorthWest = new HashSet<int>
    {
        0b00111000,
        0b01111000,
        0b00111100,
        0b01111100,
        //
        0b00111101
    };

    public static HashSet<int> wallInnerCornerNorthEast = new HashSet<int>
    {
        0b00001110,
        0b00011110,
        0b00001111,
        0b00011111,
        //
        0b01011110,

    };

    public static HashSet<int> wallInnerCornerSouthWest = new HashSet<int>
    {
        0b11110001,
        0b11100000,
        0b11110000,
        0b11100001,
        0b10100000,
        0b01010001,
        0b11010001,
        0b01100001,
        0b11010000,
        0b01110001,
        0b00010001,
        0b10110001,
        0b10100001,
        0b10010000,
        0b00110001,
        0b10110000,
        0b00100001,
        0b10010001,
        0b01011111,
        0b01001110,
    };

    public static HashSet<int> wallInnerCornerSouthEast = new HashSet<int>
    {
        0b11000111,
        0b11000011,
        0b10000011,
        0b10000111,
        0b10000010,
        0b01000101,
        0b11000101,
        0b01000011,
        0b10000101,
        0b01000111,
        0b01000100,
        0b11000110,
        0b11000010,
        0b10000100,
        0b01000110,
        0b10000110,
        0b11000100,
        0b01000010,
        0b01111101,
        0b10000001,
        0b00111101,

    };

    public static HashSet<int> wallDiagonalCornerSouthWest = new HashSet<int>
    {
        0b01000000
    };

    public static HashSet<int> wallDiagonalCornerSouthEast = new HashSet<int>
    {
        0b00000001
    };

    public static HashSet<int> wallDiagonalCornerNorthWest = new HashSet<int>
    {
        0b00010000,
        //0b01010000,
    };

    public static HashSet<int> wallDiagonalCornerNorthEast = new HashSet<int>
    {
        0b00000100,
        //0b00000101
    };

    /*
    public static HashSet<int> wallFull = new HashSet<int>
    {
        0b1101,
        0b0101,
        0b1101,
        0b1001,
        0b1111,

    };
    */

    public static HashSet<int> wallFullEightDirections = new HashSet<int>
    {
        /*
        0b11111000,
        0b11111101,
        0b11111111,
        0b11110111,
        0b01111111,
        0b00111110,
        */
        0b10001111, //peninsular soft tip west
        0b00111110, //peninsular soft tip south
        0b00111010, //peninsular soft tip south*
        0b11111000, //peninsular soft tip east
        0b11100011, //peninsular soft tip north
        0b11110011, //peninsular soft tip north
        0b11111100, //peninsular hard south to east
        0b11111001, //peninsular hard north to east
        0b11011111, //peninsular hard tip west or west hole
        0b11111101, //peninsular hard tip east or east hole
        0b01111111, //peninsular hard tip south
        0b11110111, //peninsular hard tip north
        0b01100011, //peninsular hard tip north connector
        0b00100011, //peninsular hard tip north connector
        0b11001101, //peninsular hard tip west connector
        0b10001101, //peninsular hard tip west connector
        0b10001001, //peninsular hard tip west connector
        0b11011100, //peninsular hard tip east connector**
        0b11011101, //peninsular hard tip east connector**
        0b11011000, //peninsular hard tip east connector
        0b00110110, //peninsular hard tip south connector
        0b10011100, //peninsular X connector
        0b11011000, //peninsular x connector
        0b11011101, //peninsular X connector
        0b01110111, //peninsular Y connector
        0b01100111, //peninsular Y connector
        0b00110111, //peninsular Y connector
        0b01000111, //peninsular y connector
        0b11011001, //peninsular X connector
        0b10011101, //peninsular x connector
        0b11001111, //peninsular hard north to west
        0b01111110, //peninsular hard east to south
        0b01110011, //peninsular hard east to north
        0b10011111, //peninsular hard south to west
        0b00111111, //peninsular hard west to south
        0b11100111, //peninsular hard west to north

        0b11101111, //NW diagonal fill
        0b10110011, //NW diagonal fill*
        0b11111011, //NE diagonal fill
        0b00011011, //NE diagonal fill*
        0b10001011, //SW diagonal fill

        0b11111110, //SE diagonal fill
        0b10111111, //SW diagonal fill
        0b11100110, //NW diagonal fill
        0b00101000, //SE diagonal fill

        0b11111111, //small hole
        //
        0b00010100,
        0b11100100,
        0b10010011,
        0b01110100,
        0b00010111,
        0b00010110,
        0b00110100,
        0b00010101,
        0b01010100,
        0b00010010,
        0b00100100,
        0b00010011,
        0b01100100,
        0b10010111,
        0b11110100,
        0b10010110,
        0b10110100,
        0b11100101,
        0b11010011,
        0b11110101,
        0b11010111,
        0b11010111,
        0b11110101,
        0b01110101,
        0b01010111,
        0b01100101,
        0b01010011,
        0b01010010,
        0b00100101,
        0b00110101,
        0b01010110,
        0b11010101,
        0b11010100,
        0b10010101

    };

    public static HashSet<int> wallBottmEightDirections = new HashSet<int>
    {
        0b01000001
    };
}
