#include <iostream>
#include <string>
#include <vector>
#include <map>
#include <sstream>
#include <algorithm>
#include <random>

#include <variables.h>
#include <blackbox.h>

std::map<std::pair<Material, Material>, Combination> ComAtlas; //Combination Atlas

void firstAssign(std::vector<std::pair<Material, Material>> &lhs, std::vector<Material> &rhs, std::mt19937 &random);
void secondAssign(std::vector<std::pair<Material, Material> > &lhs, std::vector<Material> &rhs, std::mt19937 &random);

int main(int argc, char *argv[]){

	std::cout << "you are Playing version: 0.1.1" << std::endl << std::endl; // VERSIONsangabe hier ändern
	std::cout << "in this game you are an alchemist and you try to create gold from scratch" << std::endl;
	std::cout << "Machines can mix two materials together in order to create other materials" << std::endl;
	std::cout << "all Combinations and efficiencies are seedbased so you have to unlock the secrets to gold all by yourself" << std::endl;
	std::cout << "type \"help\" if you're stuck" << std::endl << std::endl;

	bool quit = false;
	std::string inputBuffer;
	std::vector<std::string> input;
	unsigned int seed = 0;

	Blackbox Root(nullptr);
	std::cout << "please input seed (everything works): ";
	std::getline(std::cin, inputBuffer);
	std::cout << std::endl;
	for(unsigned int i = 0; i < inputBuffer.size(); i++){
		seed += (unsigned int)inputBuffer[i];
	}
	std::mt19937 random(seed);
	{
	std::vector<std::pair<Material, Material>> lhs;
	std::vector<Material> rhs;
	lhs.clear();
	rhs.clear();

	lhs.push_back(std::make_pair((Material)0, (Material)1));
	lhs.push_back(std::make_pair((Material)0, (Material)2));
	lhs.push_back(std::make_pair((Material)0, (Material)3));
	lhs.push_back(std::make_pair((Material)1, (Material)2));
	lhs.push_back(std::make_pair((Material)1, (Material)3));
	lhs.push_back(std::make_pair((Material)2, (Material)3));

	rhs.push_back(Helium);
	rhs.push_back(Ozon);
	rhs.push_back(Kohlenstoff);
	firstAssign(lhs, rhs, random);

	rhs.clear();

	lhs.push_back(std::make_pair((Material)4, (Material)5));
	lhs.push_back(std::make_pair((Material)4, (Material)6));
	lhs.push_back(std::make_pair((Material)5, (Material)6));
	for(int i = 0; i < 4; i++){
		lhs.push_back(std::make_pair((Material)4, (Material)i));
		lhs.push_back(std::make_pair((Material)5, (Material)i));
		lhs.push_back(std::make_pair((Material)6, (Material)i));
	}
	rhs.push_back((Material)7);
	rhs.push_back((Material)8);
	rhs.push_back((Material)9);
	rhs.push_back((Material)10);

	firstAssign(lhs, rhs, random);

	for(int i = 0; i < 7; i++){
		rhs.push_back((Material)i);
	}

	secondAssign(lhs, rhs, random);

	lhs.push_back(std::make_pair((Material)7, (Material)8));
	lhs.push_back(std::make_pair((Material)7, (Material)9));
	lhs.push_back(std::make_pair((Material)7, (Material)10));
	lhs.push_back(std::make_pair((Material)8, (Material)9));
	lhs.push_back(std::make_pair((Material)8, (Material)10));
	lhs.push_back(std::make_pair((Material)9, (Material)10));
	for(int i = 0; i < 7; i++){
		lhs.push_back(std::make_pair((Material)7, (Material)i));
		lhs.push_back(std::make_pair((Material)8, (Material)i));
		lhs.push_back(std::make_pair((Material)9, (Material)i));
		lhs.push_back(std::make_pair((Material)10, (Material)i));
	}
	rhs.push_back((Material)11);
	rhs.push_back((Material)12);
	rhs.push_back((Material)13);

	firstAssign(lhs, rhs, random);

	for(int i = 4; i < 11; i++){
		rhs.push_back((Material)i);
	}

	secondAssign(lhs, rhs, random);

	lhs.push_back(std::make_pair((Material)11, (Material)12));
	lhs.push_back(std::make_pair((Material)11, (Material)13));
	lhs.push_back(std::make_pair((Material)12, (Material)13));
	for(int i = 0; i < 11; i++){
		lhs.push_back(std::make_pair((Material)11, (Material)i));
		lhs.push_back(std::make_pair((Material)12, (Material)i));
		lhs.push_back(std::make_pair((Material)13, (Material)i));
	}
	rhs.push_back((Material)14);
	rhs.push_back((Material)15);

	firstAssign(lhs, rhs, random);

	for(int i = 7; i < 14; i++){
		rhs.push_back((Material)i);
	}

	secondAssign(lhs, rhs, random);

	lhs.push_back(std::make_pair((Material)14, (Material)15));
	for(int i = 0; i < 14; i++){
		lhs.push_back(std::make_pair((Material)14, (Material)i));
		lhs.push_back(std::make_pair((Material)15, (Material)i));
	}
	rhs.push_back((Material)16);
	rhs.push_back((Material)17);
	rhs.push_back((Material)18);

	firstAssign(lhs, rhs, random);

	for(int i = 11; i < 16; i++){
		rhs.push_back((Material)i);
	}

	secondAssign(lhs, rhs, random);

	lhs.push_back(std::make_pair((Material)16, (Material)17));
	lhs.push_back(std::make_pair((Material)16, (Material)18));
	lhs.push_back(std::make_pair((Material)17, (Material)18));
	for(int i = 0; i < 16; i++){
		lhs.push_back(std::make_pair((Material)16, (Material)i));
		lhs.push_back(std::make_pair((Material)17, (Material)i));
		lhs.push_back(std::make_pair((Material)18, (Material)i));
	}
	rhs.push_back((Material)19);
	rhs.push_back((Material)20);
	rhs.push_back((Material)21);
	rhs.push_back((Material)22);

	firstAssign(lhs, rhs, random);

	for(int i = 14; i < 19; i++){
		rhs.push_back((Material)i);
	}

	secondAssign(lhs, rhs, random);

	lhs.push_back(std::make_pair((Material)19, (Material)20));
	lhs.push_back(std::make_pair((Material)19, (Material)21));
	lhs.push_back(std::make_pair((Material)19, (Material)22));
	lhs.push_back(std::make_pair((Material)20, (Material)21));
	lhs.push_back(std::make_pair((Material)20, (Material)21));
	lhs.push_back(std::make_pair((Material)21, (Material)22));
	for(int i = 0; i < 19; i++){
		lhs.push_back(std::make_pair((Material)19, (Material)i));
		lhs.push_back(std::make_pair((Material)20, (Material)i));
		lhs.push_back(std::make_pair((Material)21, (Material)i));
		lhs.push_back(std::make_pair((Material)22, (Material)i));
	}
	rhs.push_back((Material)23);
	rhs.push_back((Material)24);
	rhs.push_back((Material)25);

	firstAssign(lhs, rhs, random);

	for(int i = 16; i < 23; i++){
		rhs.push_back((Material)i);
	}

	secondAssign(lhs, rhs, random);

	lhs.push_back(std::make_pair((Material)23, (Material)24));
	lhs.push_back(std::make_pair((Material)23, (Material)25));
	lhs.push_back(std::make_pair((Material)24, (Material)25));
	for(int i = 0; i < 23; i++){
		lhs.push_back(std::make_pair((Material)23, (Material)i));
		lhs.push_back(std::make_pair((Material)24, (Material)i));
		lhs.push_back(std::make_pair((Material)25, (Material)i));
	}
	rhs.push_back((Material)26);
	rhs.push_back((Material)27);

	firstAssign(lhs, rhs, random);

	for(int i = 19; i < 26; i++){
		rhs.push_back((Material)i);
	}

	secondAssign(lhs, rhs, random);

	lhs.push_back(std::make_pair((Material)26, (Material)27));
	for(int i = 0; i < 26; i++){
		lhs.push_back(std::make_pair((Material)26, (Material)i));
		lhs.push_back(std::make_pair((Material)27, (Material)i));
	}
	rhs.push_back((Material)28);

	firstAssign(lhs, rhs, random);

	for(int i = 23; i < 28; i++){
		rhs.push_back((Material)i);
	}

	secondAssign(lhs, rhs, random);

	}

	Container In1;
	Container In2;
	Container In3;
	Container In4;
	Container Out;

	In1.Amount = In2.Amount = In3.Amount = In4.Amount = InAmount;
	In1.Content = Feuer; In2.Content = Wasser; In3.Content = Erde; In4.Content = Luft;

	Out.Amount = 0;
	Out.Content = Gold;
	Root.AddOut(&Out);
	Root.AddIn(&In1);
	Root.AddIn(&In2);
	Root.AddIn(&In3);
	Root.AddIn(&In4);

	Blackbox* Current;
	Current = &Root;
#ifndef NDEBUG
	std::cout << "type in antything to continue: ";
	std::cin >> inputBuffer;
#endif
	inputBuffer.clear();
	Current->render();

	while (!quit){
		input.clear();

		std::getline(std::cin, inputBuffer);
		std::istringstream tmp(inputBuffer);
		inputBuffer.clear();
		while(tmp.good()){
			tmp >> inputBuffer;
			input.push_back(inputBuffer);
		}

#ifdef _WIN32
		system("cls");
#else
		system("clear");
#endif

		if(input[0] == "exit"){
			quit = true;
		}else if (input[0] == "quit"){
			quit = true;
		}else if (input[0] == "help"){
			if(input.size() > 1){
				if(input[1] == "help"){
					std::cout << "are you fucking kidding me?" << std::endl;
				}else if(input[1] == "exit" || input[1] == "quit"){
					std::cout << "there is absolutely nothing here" << std::endl;
				}else if(input[1] == "rules"){
					std::cout << "\"rules\" shows some rules of the game" << std::endl;
				}else if(input[1] == "next"){
					std::cout << "\"next\" does nothing but processing next tick" << std::endl;
				}else if(input[1] == "cd"){
					std::cout << "\"cd [../Name]\" enter a Blackbox or go back up ... as already said" << std::endl;
				}else if(input[1] == "add"){
					std::cout << "\"add Machine [Prime input index] [Secondary input index]\"" << std::endl;
					std::cout << "or \"add Machine [Prime input index] [Secondary input index] [Prime Output index] [Secondary Output index]\" to add a Machine." << std::endl;
					std::cout << "\"add Blackbox [Name]\" adds a new Blackbox in your Blackbox" << std::endl;
					std::cout << "\"add Input [Blackbox Name] [Input index]\" adds an Input to the given Blackbox" << std::endl;
					std::cout << "\"add Output [Blackbox Name] [Input index]\" adds an Output to the given Blackbox" << std::endl;
				}else if(input[1] == "change"){
					std::cout << "\"change Machine [Machine index]\" changes state of machine (on/off)" << std::endl;
					std::cout << "\"change Input [Blackbox index] [Old Input index] [New Input index]\" changes the input" << std::endl;
					std::cout << "\"change Output [Blackbox name] [Old Output index] [New Output index]\" changes the output" << std::endl;
					std::cout << "\"change Ratio [Machine index] [New Ratio]\" changes the ratio" << std::endl;
					std::cout << "\"change Name [Old Blackbox name] [new Blackbox name]\" will overwrite blackbox if name already exists" << std::endl;
				}else if(input[1] == "delete"){
					std::cout << "\"delete Machine [Machine index]\" deletes machine with this index" << std::endl;
					std::cout << "\"delete Blackbox [Blackbox name]\" deletes blackbox with this name" << std::endl;
					std::cout << "\"delete Input [Blackbox name] [Input index]\" deletes the input" << std::endl;
					std::cout << "\"delete Output [Blackbox name] [Output index]\" deletes the output" << std::endl;
					std::cout << "\"delete Container [Container index]\" deletes the container" << std::endl;
				}else
					std::cout << "what ever you typed in. it is no command" << std::endl;
			}else{
				std::cout << "mix two Materials to get a new one" << std::endl;
				std::cout << "\"rules\" shows you the underlying rules, the game is working." << std::endl;
				std::cout << "\"help [command]\" shows specific help" << std::endl;
				std::cout << "\"exit\" or \"quit\" to quit the game" << std::endl;
				std::cout << "\"cd [Name/..]\" enter a Blackbox or go back up. \"cd ..\" in the most upper stage will exit the game." << std::endl;
				std::cout << "\"add [Machine/Blackbox/Input/Output]\" add something" << std::endl;
				std::cout << "\"change [Machine/Input/Output/Ratio/Name]\" changes something" << std::endl;
				std::cout << "\"delete [Machine/Blackbox/Input/Output/Container]\" deletes something" << std::endl;
				std::cout << "\"next\" only this will process everything. otherwise the game time will be frozen." << std::endl;
			}
		}else if (input[0] == "rules"){
			std::cout << "all materials have a refinement degree" << std::endl;
			std::cout << "0. Feuer, Wasser, Erde, Luft" << std::endl;
			std::cout << "1. Helium, Ozon, Kohlenstoff" << std::endl;
			std::cout << "2. Methanol, Propan, Ethan, Acetaldehyd" << std::endl;
			std::cout << "3. Polyvinylchlorid, Polytetrafluorethylen, Siliciumdioxid" << std::endl;
			std::cout << "4. Plastik, Glas" << std::endl;
			std::cout << "5. Stahl, Aluminium, Messing" << std::endl;
			std::cout << "6. Plutonium, Radon, Uran, Fermium" << std::endl;
			std::cout << "7. Silicium, Wolfram, Silber" << std::endl;
			std::cout << "8. Quecksilber, Platin" << std::endl;
			std::cout << "9. Gold" << std::endl;
			std::cout << "the goal is to create gold by combining two materials but" << std::endl;
			std::cout << "by combining them in a machine you can reach a maximum of one degree higher as the best material in your combination." << std::endl;
			std::cout << "different materials have different efficiencies and the ratio of waste and product can also vary." << std::endl;
			std::cout << "e.g. the combination of \"Feuer\" and \"Wasser\" in your seed creates " << GetName(ComAtlas[std::make_pair(Feuer, Wasser)].OutPrime) << " and as a waste product " << GetName(ComAtlas[std::make_pair(Feuer, Wasser)].OutSecondary) << "." << std::endl;
			std::cout << "the best efficiency is by " << (float)ComAtlas[std::make_pair(Feuer, Wasser)].XMax/16 * 100 << "% and it will use up " <<
						 (float)ComAtlas[std::make_pair(Feuer, Wasser)].YMax/16 * MaxOutput << " materials. The default ratio is " <<
						 ((float)ComAtlas[std::make_pair(Feuer, Wasser)].YCurve/2 + 8)/16 * 100 << "% but the best/worst case is at " <<
						 (float)ComAtlas[std::make_pair(Feuer, Wasser)].XCurve/16 * 100 << "% with an amplitude of " <<
						 ((float)ComAtlas[std::make_pair(Feuer, Wasser)].AmpCurve/2)/16 + ((float)ComAtlas[std::make_pair(Feuer, Wasser)].YCurve/2 + 8)/16 * 100 << "% (" << (float)ComAtlas[std::make_pair(Feuer, Wasser)].AmpCurve << "). this effect will have more or less impact on the surrounding ratios given by the standard deviation. it can range between 0 and 15. in this case it is " <<
						 ComAtlas[std::make_pair(Feuer, Wasser)].DevCurve << std::endl;
			std::cout << "use blackboxes to clean up your screen and manage machines and containers in different/seperated areas." << std::endl;
		}else if(input[0] == "next" || input[0] == "n" || input[0] == "N"){
			Root.process();
			In1.Amount += InAmount;
			In2.Amount += InAmount;
			In3.Amount += InAmount;
			In4.Amount += InAmount;
#ifndef NDEBUG
		}else if(input[0] == "GETATLAS"){
			for(auto && it : ComAtlas){
				std::cout << GetName(it.first.first) << ":" << GetName(it.first.second) << " = " << GetName(it.second.OutPrime) << ":" << GetName(it.second.OutSecondary) << "; Max = (" << it.second.XMax << "|" << it.second.YMax << "), Curve = (" << it.second.XCurve << "|" << it.second.YCurve << "), Amp = " << it.second.AmpCurve << " and Standard Deviation = " << it.second.DevCurve << std::endl;
			}
#endif
		}else{
			Blackbox* ret = Current->input(input);
			if (ret)
				Current = ret;
			else
				quit = true;
		}

		std::cout << "input: ";
		for (unsigned int i = 0; i < input.size(); i++){
			std::cout << input[i] << " ";
		}std::cout << std::endl;
		std::cout << "===== ===== ===== ===== =====" << std::endl;

		Current->render();
	}

	Root.~Blackbox();
	return 0;
}

void firstAssign(std::vector<std::pair<Material, Material> > &lhs, std::vector<Material> &rhs, std::mt19937 &random){
	Combination temp;
	std::shuffle(lhs.begin(), lhs.end(), random);
	for (unsigned int i = 0; i < rhs.size(); i++){
		temp.OutPrime = rhs[(int)i];
#ifndef NDEBUG
		std::cout << "=/= " << GetName(lhs.back().first) << ":" << GetName(lhs.back().second) << " = " << GetName(temp.OutPrime) << " (" << GetName(rhs[(int)i]) << ") Nr." << i << std::endl;
#endif
		temp.OutSecondary = static_cast<Material>(random()%(rhs.back()+1));
		temp.XMax = random()%16;
		temp.YMax = random()%16;
		temp.XCurve = random()%16;
		temp.YCurve = random()%16;
		temp.DevCurve = random()%16;
		temp.AmpCurve = (random()%16)-8;
		if(temp.AmpCurve+temp.YCurve < 0){
			temp.AmpCurve = -temp.YCurve;
		}else if(temp.AmpCurve+temp.YCurve > 16){
			temp.AmpCurve = 16-temp.YCurve;
		}
		ComAtlas[lhs.back()] = temp;
		lhs.pop_back();

		temp.OutSecondary = static_cast<Material>(random()%(rhs.back()+1));
		temp.XMax = random()%16;
		temp.YMax = random()%16;
		temp.XCurve = random()%16;
		temp.YCurve = random()%16;
		temp.DevCurve = random()%16;
		temp.AmpCurve = (random()%16)-8;
		if(temp.AmpCurve+temp.YCurve < 0){
			temp.AmpCurve = -temp.YCurve;
		}else if(temp.AmpCurve+temp.YCurve > 16){
			temp.AmpCurve = 16-temp.YCurve;
		}
		ComAtlas[lhs.back()] = temp;
		lhs.pop_back();
	}
}

void secondAssign(std::vector<std::pair<Material, Material> > &lhs, std::vector<Material> &rhs, std::mt19937 &random){
	Combination temp;
	std::shuffle(rhs.begin(), rhs.end(), random);
	for (unsigned int i = 0; !lhs.empty(); i++){
		temp.OutPrime = rhs[(int)(i%rhs.size())];
#ifndef NDEBUG
		std::cout << "=== " << GetName(lhs.back().first) << ":" << GetName(lhs.back().second) << " = " << GetName(temp.OutPrime) << " (" << GetName(rhs[(int)(i%rhs.size())]) << ") Nr." << (i%rhs.size()) << std::endl;
#endif
		temp.OutSecondary = static_cast<Material>(random()%(rhs.back()+1));
		temp.XMax = random()%16;
		temp.YMax = random()%16;
		temp.XCurve = random()%16;
		temp.YCurve = random()%16;
		temp.DevCurve = random()%16;
		temp.AmpCurve = (random()%16)-8;
		if(temp.AmpCurve+temp.YCurve < 0){
			temp.AmpCurve = -temp.YCurve;
		}else if(temp.AmpCurve+temp.YCurve > 16){
			temp.AmpCurve = 16-temp.YCurve;
		}
		ComAtlas[lhs.back()] = temp;
		lhs.pop_back();
	}
	rhs.clear();
}
