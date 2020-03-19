#pragma once
#include <utility>
#include <map>
#include <string>

#ifndef M_PI
#define M_PI		3.14159265358979323846
#endif
#define MaxOutput 50
#define InAmount 50

#ifdef _WIN32
#define UR '\xda'
#define LOR '\xc1'
#define LR '\xc4'
#define LU '\xbf'
#define OU '\xb3'
#define OR '\xc0'
#define LUR '\xc2'
#define LO '\xd9'
#define ORU '\xc3'
#define ORUL '\xc5'
#else
#define UR "┌"
#define LOR "┴"
#define LR "─"
#define LU "┐"
#define OU "│"
#define OR "└"
#define LUR "┬"
#define LO "┘"
#define ORU "├"
#define ORUL "┼"
#endif

enum Material: int {Feuer = 0, Wasser = 1, Erde = 2, Luft = 3,
					Helium = 4, Ozon = 5, Kohlenstoff = 6,
					Methanol = 7, Propan = 8, Ethan = 9, Acetaldehyd = 10,
					Polyvinylchlorid = 11, Polytetrafluorethylen = 12, Siliciumdioxid = 13,
					Plastik = 14, Glas = 15,
					Stahl = 16, Aluminium = 17, Messing = 18,
					Plutonium = 19, Radon = 20, Uran = 21, Fermium = 22,
					Silicium = 23, Wolfram = 24, Silber = 25,
					Quecksilber = 26, Platin = 27,
					Gold = 28};

typedef struct{
	unsigned int PrimeUsage;
	unsigned int SecondaryUsage;
	std::pair<Material, unsigned int> Prime;
	std::pair<Material, unsigned int> Secondary;
}InfMach; //interface Machine

typedef struct{
	Material OutPrime;
	Material OutSecondary;
	int XMax;
	int YMax;
	int XCurve;
	int YCurve;
	int AmpCurve;
	int DevCurve;
}Combination;

typedef struct{
	Material Content;
	unsigned int Amount;
}Container;

extern std::map<std::pair<Material, Material>, Combination> ComAtlas; //Combination Atlas

inline std::string GetName(const Material& M){
	switch (M) {
	case Feuer:
		return "Feuer";
	case Wasser:
		return "Wasser";
	case Erde:
		return "Erde";
	case Luft:
		return "Luft";
	case Helium:
		return "Helium";
	case Ozon:
		return "Ozon";
	case Kohlenstoff:
		return "Kohlenstoff";
	case Methanol:
		return "Methanol";
	case Propan:
		return "Propan";
	case Ethan:
		return "Ethan";
	case Acetaldehyd:
		return "Acetaldehyd";
	case Polyvinylchlorid:
		return "PVC";
	case Polytetrafluorethylen:
		return "PTFE ";
	case Siliciumdioxid:
		return "SiO2";
	case Plastik:
		return "Plastik";
	case Glas:
		return "Glas";
	case Stahl:
		return "Stahl";
	case Aluminium:
		return "Aluminium";
	case Messing:
		return "Messing";
	case Plutonium:
		return "Plutonium";
	case Radon:
		return "Radon";
	case Uran:
		return "Uran";
	case Fermium:
		return "Fermium";
	case Silicium:
		return "Silicium";
	case Wolfram:
		return "Wolfram";
	case Silber:
		return "Silber";
	case Quecksilber:
		return "Quecksilber";
	case Platin:
		return "Platin";
	case Gold:
		return "Gold";
	default:
		return "unknown";
	}
}
