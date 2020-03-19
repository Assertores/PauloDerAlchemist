#include "blackbox.h"

Blackbox::Blackbox(Blackbox *Parent){
	Father = Parent;
	LastMachineIndex = -1;
	LastContainerIndex = -1;
}

Blackbox::~Blackbox(){
	for (auto&& it : BlackboxAtlas){
		delete it.second;
	}
	In.clear();
	Out.clear();
	MachineAtlas.clear();
	ContainerAtlas.clear();
}

Blackbox* Blackbox::input(std::vector<std::string> input){
	if(input[0] == "cd"){
		if (input.size() < 2){
			std::cout << "nothing to go to" << std::endl;
			return this;
		}
		if(input[1] == ".."){
			return Father;
		}else if(BlackboxAtlas.count(input[1])){
			return BlackboxAtlas[input[1]];
		}else{
			std::cout << "Blackbox not found" << std::endl;
			return this;
		}
	}else if(input[0] == "add"){
		if(input[1] == "Machine" || input[1] == "machine"){
			InfMach tmp;
			if(input.size() < 4){
				std::cout << "not enough arguments" << std::endl;
				return this;
			}
			try{tmp.Prime.second = std::stoi(input[2]);}catch(std::invalid_argument){
				std::cout << "Prime input " << input[2] << " is no numerical value" << std::endl;
				return this;
			}
			try{tmp.Secondary.second = std::stoi(input[3]);}catch(std::invalid_argument){
				std::cout << "Secondary input " << input[3] << " is no numerical value" << std::endl;
				return this;
			}
			if(input.size() > 4){
				try{tmp.PrimeUsage = std::stoi(input[4]);}catch(std::invalid_argument){
					std::cout << "Prime output " << input[4] << " is no numerical value" << std::endl;
					return this;
				}
				try{tmp.SecondaryUsage = std::stoi(input[5]);}catch(std::invalid_argument){
					std::cout << "Secondary output " << input[5] << " is no numerical value" << std::endl;
					return this;
				}
				if(tmp.PrimeUsage > (unsigned)LastContainerIndex){
					LastContainerIndex++;
					tmp.PrimeUsage = LastContainerIndex;
				}
				if(tmp.SecondaryUsage > (unsigned)LastContainerIndex){
					LastContainerIndex++;
					tmp.SecondaryUsage = LastContainerIndex;
				}
				tmp.Prime.first = ComAtlas[std::make_pair(ContainerAtlas[tmp.Prime.second].Content,ContainerAtlas[tmp.Secondary.second].Content)].OutPrime;
				tmp.Secondary.first = ComAtlas[std::make_pair(ContainerAtlas[tmp.Prime.second].Content,ContainerAtlas[tmp.Secondary.second].Content)].OutSecondary;
			}else{
				if(ComAtlas.find(std::make_pair(ContainerAtlas[tmp.Prime.second].Content,ContainerAtlas[tmp.Secondary.second].Content)) == ComAtlas.end()){
					auto temp = tmp.Prime;
					tmp.Prime = tmp.Secondary;
					tmp.Secondary = temp;
				}
					Container temp;
					temp.Amount = 0;
					temp.Content = ComAtlas[std::make_pair(ContainerAtlas[tmp.Prime.second].Content,ContainerAtlas[tmp.Secondary.second].Content)].OutPrime;
					LastContainerIndex ++;
					ContainerAtlas[LastContainerIndex] = temp;
					tmp.Prime.first = temp.Content;
					tmp.PrimeUsage = LastContainerIndex;
					temp.Content = ComAtlas[std::make_pair(ContainerAtlas[tmp.Prime.second].Content,ContainerAtlas[tmp.Secondary.second].Content)].OutSecondary;
					LastContainerIndex ++;
					ContainerAtlas[LastContainerIndex] = temp;
					tmp.Secondary.first = temp.Content;
					tmp.SecondaryUsage = LastContainerIndex;
			}
			LastMachineIndex++;
			MachineAtlas.emplace(std::make_pair(LastMachineIndex, tmp));
		}else if(input[1] == "Blackbox" || input[1] == "blackbox"){
			if (input.size() < 3){
				std::cout << "Blackbox has no name" << std::endl;
				return this;
			}
			Blackbox* temp = new Blackbox(this);
			BlackboxAtlas.emplace(std::make_pair(input[2], temp));
		}else if((input[1] == "Input" || input[1] == "input") && input.size() > 3){
			if(input.size() < 4){
				std::cout << "not enough arguments" << std::endl;
				return this;
			}
			if(BlackboxAtlas.find(input[2]) == BlackboxAtlas.end()){
				std::cout << "Blackbox " << input[2] << " does not exist" << std::endl;
				return this;
			}
			try{std::stoi(input[3]);}catch(std::invalid_argument){
				std::cout << "Input index " << input[3] << " is no numerical value" << std::endl;
				return this;
			}
			if (ContainerAtlas.find(std::stoi(input[3])) != ContainerAtlas.end()){
				BlackboxAtlas[input[2]]->AddIn(&ContainerAtlas[std::stoi(input[3])]);
			}else{
				std::cout << "Container " << input[3] << " does not exist" << std::endl;
			}
		}else if((input[1] == "Output" || input[1] == "output") && input.size() > 3){
			if(input.size() < 4){
				std::cout << "not enough arguments" << std::endl;
				return this;
			}
			if(BlackboxAtlas.find(input[2]) == BlackboxAtlas.end()){
				std::cout << "Blackbox " << input[2] << " does not exist" << std::endl;
				return this;
			}
			try{BlackboxAtlas[input[2]]->AddOut(&ContainerAtlas[std::stoi(input[3])]);}catch(std::invalid_argument){
				std::cout << "Output index " << input[3] << " is no numerical value" << std::endl;
				return this;
			}
		}else{
			std::cout << input[1] << " can't be added" << std::endl;
		}
	}else if(input[0] == "change"){
		if(input[1] == "Input" || input[1] == "input"){
			if(input.size() < 5){
				std::cout << "not enough arguments" << std::endl;
				return this;
			}
			if(BlackboxAtlas.find(input[2]) != BlackboxAtlas.end()){
				try{std::stoi(input[3]);}catch(std::invalid_argument){
					std::cout << "Input index " << input[3] << " is no numerical value" << std::endl;
					return this;
				}
				auto iterator = BlackboxAtlas[input[2]]->In.find(&ContainerAtlas[std::stoi(input[3])]);
				if(iterator == BlackboxAtlas[input[2]]->In.end()){
					std::cout << "Input index " << input[3] << " is no existing input" << std::endl;
					return this;
				}
				try{std::stoi(input[4]);}catch(std::invalid_argument){
					std::cout << "new Input index " << input[4] << " is no numerical value" << std::endl;
					return this;
				}
				if (ContainerAtlas.find(std::stoi(input[4])) == ContainerAtlas.end()){
					std::cout << "new Input index " << input[4] << " is no existing container" << std::endl;
					return this;
				}
				auto temp = BlackboxAtlas[input[2]]->In[&ContainerAtlas[std::stoi(input[3])]];
				BlackboxAtlas[input[2]]->In.erase(&ContainerAtlas[std::stoi(input[3])]);
				BlackboxAtlas[input[2]]->In[&ContainerAtlas[std::stoi(input[4])]] = temp;
			}else{
				std::cout << "Blackbox " << input[2] << " does not exist" << std::endl;
			}
		}else if((input[1] == "Output" || input[1] == "output") && input.size() > 4){
			if(input.size() < 5){
				std::cout << "not enough arguments" << std::endl;
				return this;
			}
			if(BlackboxAtlas.find(input[2]) != BlackboxAtlas.end()){
				try{std::stoi(input[3]);}catch(std::invalid_argument){
					std::cout << "Output index " << input[3] << " is no numerical value" << std::endl;
					return this;
				}
				Container* iterator = nullptr;
				for (auto&& it : BlackboxAtlas[input[2]]->Out){
					if(it.second == &ContainerAtlas[std::stoi(input[3])]){
						iterator = it.first;
					}
				}
				if(!iterator){
					std::cout << "Output index " << input[3] << " is no existing input" << std::endl;
					return this;
				}
				try{std::stoi(input[4]);}catch(std::invalid_argument){
					std::cout << "new Output index " << input[4] << " is no numerical value" << std::endl;
					return this;
				}
				if (ContainerAtlas.find(std::stoi(input[4])) == ContainerAtlas.end()){
					std::cout << "new Output index " << input[4] << " is no existing container" << std::endl;
					return this;
				}
				BlackboxAtlas[input[2]]->Out[iterator] = &ContainerAtlas[std::stoi(input[4])];
				*iterator = ContainerAtlas[std::stoi(input[4])];
			}else{
				std::cout << "Blackbox " << input[2] << " does not exist" << std::endl;
			}
		}else if((input[1] == "Ratio" || input[1] == "ratio") && input.size() > 3){
			if(input.size() < 4){
				std::cout << "not enough arguments" << std::endl;
				return this;
			}
			int index;
			try{index = std::stoi(input[2]);}catch(std::invalid_argument){
				std::cout << "Machine index " << input[2] << " is no numerical value" << std::endl;
				return this;
			}
			auto tmp = MachineAtlas.find(index);
			if(tmp != MachineAtlas.end()){
				try{tmp->second.ChangeRatio(std::stoi(input[3]));}catch(std::invalid_argument){
					std::cout << "Ratio " << input[3] << " is no numerical value" << std::endl;
					return this;
				}
			}else{
				std::cout << "Machine index " << input[2] << " does not exist" << std::endl;
			}
		}else if((input[1] == "Name" || input [1] == "name" || input[1] == "Blackbox" || input[1] == "blackbox") && input.size() > 3){
			if(input.size() < 4){
				std::cout << "not enough arguments" << std::endl;
				return this;
			}
			if(BlackboxAtlas.find(input[2]) != BlackboxAtlas.end()){
				BlackboxAtlas[input[3]] = BlackboxAtlas[input[2]];
				BlackboxAtlas.erase(input[2]);
			}else{
				std::cout << "Blackbox " << input[2] << " does not exist" << std::endl;
			}
		}else if((input[1] == "Machine" || input[1] == "machine") && input.size() > 2){
			try{std::stoi(input[2]);}catch(std::invalid_argument){
				std::cout << "machine index " << input[2] << " is no numerical value" << std::endl;
				return this;
			}
			if(MachineAtlas.find(std::stoi(input[2])) == MachineAtlas.end()){
				std::cout << "machine " << input[2] << " dose not exist" << std::endl;
				return this;
			}
			auto it = MachineAtlas.find(std::stoi(input[2]));
			if(it != MachineAtlas.end()){
				if (it->second.on){
					it->second.on = false;
				}else{
					it->second.on = true;
				}
			}
		}else{
			std::cout << input[1] << " can't be changed" << std::endl;
		}
	}else if(input[0] == "delete"){
		if(input[1] == "Machine" || input[1] == "machine"){
			if(input.size() < 3){
				std::cout << "not enough arguments" << std::endl;
				return this;
			}
			try{MachineAtlas.erase(std::stoi(input[2]));}catch(std::invalid_argument){
				std::cout << "Maschin index " << input[2] << " is no numerical value" << std::endl;
				return this;
			}
		}else if(input[1] == "Blackbox" || input[1] == "blackbox"){
			if(input.size() < 3){
				std::cout << "not enough arguments" << std::endl;
				return this;
			}
			if (BlackboxAtlas.find(input[2]) != BlackboxAtlas.end()){
				delete BlackboxAtlas[input[2]];
				BlackboxAtlas.erase(input[2]);
			}else{
				std::cout << "Blackbox " << input[2] << " does not exist" << std::endl;
			}
		}else if(input[1] == "Input" || input[1] == "input"){
			if(input.size() < 4){
				std::cout << "not enough arguments" << std::endl;
				return this;
			}
			if (BlackboxAtlas.find(input[2]) != BlackboxAtlas.end()){
				try{std::stoi(input[3]);}catch(std::invalid_argument){
					std::cout << "Input index " << input[3] << " is no numerical value" << std::endl;
					return this;
				}
				if (BlackboxAtlas[input[2]]->In.find(&ContainerAtlas[std::stoi(input[3])]) != BlackboxAtlas[input[2]]->In.end()){
					BlackboxAtlas[input[2]]->DeleteIn(&ContainerAtlas[std::stoi(input[3])]);
				}else{
					std::cout << "Input index " << input[3] << " does not exist" << std::endl;
				}
			}else{
				std::cout << "Blackbox " << input[2] << " does not exist" << std::endl;
			}
		}else if(input[1] == "Output" || input[1] == "output"){
			if(input.size() < 4){
				std::cout << "not enough arguments" << std::endl;
				return this;
			}
			if (BlackboxAtlas.find(input[2]) != BlackboxAtlas.end()){
				try{std::stoi(input[3]);}catch(std::invalid_argument){
					std::cout << "Output index " << input[3] << " is no numerical value" << std::endl;
					return this;
				}
				Container* index = nullptr;
				for (auto&& it : BlackboxAtlas[input[2]]->Out){
					if (it.second == &ContainerAtlas[std::stoi(input[3])]){
						index = it.first;
					}
				}
				if(index != nullptr){
					BlackboxAtlas[input[2]]->DeleteOut(index);
				}else{
					std::cout << "Input " << input[3] << " is no valid Input" << std::endl;
				}
			}else{
				std::cout << "Blackbox " << input[2] << " does not exist" << std::endl;
			}
		}else if(input[1] == "Container" || input[1] == "container"){
			if(input.size() < 3){
				std::cout << "not enough arguments" << std::endl;
				return this;
			}
			try{std::stoi(input[2]);}catch(std::invalid_argument){
				std::cout << "Container index " << input[2] << " is no numerical value" << std::endl;
				return this;
			}
			if (ContainerAtlas.find(std::stoi(input[2])) == ContainerAtlas.end()){
				std::cout << "Container " << input[2] << " does not exist" << std::endl;
				return this;
			}
			if(Out.find(&ContainerAtlas[std::stoi(input[2])]) != Out.end()){
				std::cout << "Container " << input[2] << " is one of the Outputs" << std::endl;
				return this;
			}
			for (auto&& it : In){
				if (it.second == &ContainerAtlas[std::stoi(input[2])]){
					std::cout << "Container " << input[2] << " is one of the Inputs" << std::endl;
					return this;
				}
			}
			ContainerAtlas.erase(std::stoi(input[2]));
		}else{
			std::cout << input[1] << " can't be deleted" << std::endl;
		}
	}else{
		std::cout << "command: " << input[0] << " unknown!" << std::endl;
	}
	return this;
}

void Blackbox::process(){
	for (auto&& it : In){
		it.second->Content = it.first->Content;
		it.second->Amount = it.first->Amount;
	}
	for (auto&& it : Out){
		it.first->Content = it.second->Content;
		it.first->Amount = it.second->Amount;
	}

	for (auto&& it : BlackboxAtlas){
		it.second->process();
	}
	InfMach tmp;
	for (auto&& it : MachineAtlas){
		tmp = it.second.process(ContainerAtlas[it.second.InPrimeIndex], ContainerAtlas[it.second.InSecondaryIndex]);
		if (ContainerAtlas[it.second.OutPrimeIndex].Content != tmp.Prime.first){
			if(ContainerAtlas[it.second.OutPrimeIndex].Amount == 0){
				ContainerAtlas[it.second.OutPrimeIndex].Content = tmp.Prime.first;
			}else{
				std::cout << "MachineIndex:	" << it.first << std::endl;
				std::cout << "Prime output (" << GetName(tmp.Prime.first) << ") does not match Container contant (" << GetName(ContainerAtlas[it.second.OutPrimeIndex].Content) << ")" << std::endl;
			}
		}else if (ContainerAtlas[it.second.OutSecondaryIndex].Content != tmp.Secondary.first){
			if(ContainerAtlas[it.second.OutSecondaryIndex].Amount == 0){
				ContainerAtlas[it.second.OutSecondaryIndex].Content = tmp.Secondary.first;
			}else{
				std::cout << "MachineIndex:	" << it.first << std::endl;
				std::cout << "Secondary output (" << GetName(tmp.Prime.first) << ") does not match Container contant (" << GetName(ContainerAtlas[it.second.OutPrimeIndex].Content) << ")" << std::endl;
			}
		}else{
			ContainerAtlas[it.second.InPrimeIndex].Amount -= tmp.PrimeUsage;
			ContainerAtlas[it.second.InSecondaryIndex].Amount -= tmp.SecondaryUsage;
			ContainerAtlas[it.second.OutPrimeIndex].Amount += tmp.Prime.second;
			ContainerAtlas[it.second.OutSecondaryIndex].Amount += tmp.Secondary.second;
		}
	}
	for (auto&& it : In){
		it.first->Content = it.second->Content;
		it.first->Amount = it.second->Amount;
	}
	for (auto&& it : Out){
		it.second->Content = it.first->Content;
		it.second->Amount = it.first->Amount;
	}
}

void Blackbox::render(){
	std::cout << "Inputs:" << std::endl;
	for (auto&& it : In){
		for(auto && da : ContainerAtlas){
			if(&da.second == it.second){
				std::cout << da.first << ":	";
				break;
			}
		}
		std::cout << it.second->Amount << "	" << GetName(it.second->Content) << std::endl;
	}
	std::cout << "Output:" << std::endl;
	for (auto&& it : Out){
		for(auto && da : ContainerAtlas){
			if(&da.second == it.first){
				std::cout << da.first << ":	";
				break;
			}
		}
		std::cout << it.second->Amount << "	" << GetName(it.second->Content) << std::endl;
	}
	std::cout << "Containers:" << std::endl;
	for (auto&& it : ContainerAtlas){
		std::cout << it.first << ":	" << it.second.Amount << "	" << GetName(it.second.Content) << std::endl;
	}
	std::cout << "===== ===== ===== ===== =====" << std::endl;
	for (unsigned int i = 0; i < ContainerAtlas.size(); i++){
		std::cout << i << " ";
	}std::cout << std::endl;
	for (unsigned int i = 0; i < ContainerAtlas.size(); i++){
		std::cout << OU << " ";
	}std::cout << std::endl;

	for (auto&& it : BlackboxAtlas){
		{
			unsigned int i = it.second->In.size();
			for (auto && ma : it.second->In){
				bool hit = false;
				for (auto && da : ContainerAtlas){
					if(&da.second == ma.first){
						std::cout << ORU << LR;
						hit = true;
					}else if(hit){
						std::cout << ORUL << LR;
					}else{
						std::cout << OU << " ";
					}
				}
				unsigned int j = 1;
				for (; j < i; j++){
					std::cout << LR << LR << LR << LR << LR << LR << LR << LR << LR << LR << LR;
				}
				std::cout << LR << LU;
				for (;j < it.second->In.size(); j++){
					std::cout << "          " << OU;
				}std::cout << std::endl;
				i--;
			}
		}
		for (unsigned int i = 0; i < ContainerAtlas.size(); i++){
			std::cout << OU << " ";
		}

		std::cout << UR << LOR << LR << LR << LR << LR << LR << LR << LR << LR << LR << LR;
		for (unsigned int i = 1; i < it.second->In.size(); i++){
			std::cout << LOR << LR << LR << LR << LR << LR << LR << LR << LR << LR << LR;
		}std::cout << LU << std::endl;

		for (unsigned int i = 0; i < ContainerAtlas.size(); i++){
			std::cout << OU << " ";
		}

		std::cout << OU;
		for (auto && ma : it.second->In){
			std::cout << std::left << std::setw(11) << GetName(ma.second->Content);
		}std::cout << OU << std::endl;

		for (unsigned int i = 0; i < ContainerAtlas.size(); i++){
			std::cout << OU << " ";
		}

		std::cout << OU;
		for (auto && ma : it.second->In){
			std::cout << std::left << std::setw(11) << ma.second->Amount;
		}std::cout << OU << std::endl;

		for (unsigned int i = 0; i < ContainerAtlas.size(); i++){
			std::cout << OU << " ";
		}

		std::cout << OU << it.first << std::endl;

		for (unsigned int i = 0; i < ContainerAtlas.size(); i++){
			std::cout << OU << " ";
		}

		std::cout << OU;
		for (auto && ma : it.second->Out){
			std::cout << std::left << std::setw(11) << ma.second->Amount;
		}std::cout << OU << std::endl;

		for (unsigned int i = 0; i < ContainerAtlas.size(); i++){
			std::cout << OU << " ";
		}

		std::cout << OU;
		for (auto && ma : it.second->Out){
			std::cout << std::left << std::setw(11) << GetName(ma.second->Content);
		}std::cout << OU << std::endl;

		for (unsigned int i = 0; i < ContainerAtlas.size(); i++){
			std::cout << OU << " ";
		}

		std::cout << OR << LUR << LR << LR << LR << LR << LR << LR << LR << LR << LR << LR;
		for (unsigned int i = 1; i < it.second->Out.size(); i++){
			std::cout << LUR << LR << LR << LR << LR << LR << LR << LR << LR << LR << LR;
		}std::cout << LO << std::endl;

		{
			unsigned int i = 0;
			for (auto && ma : it.second->Out){
				bool hit = false;
				for(auto && da : ContainerAtlas){
					if(&da.second == ma.second){
						std::cout << ORU << LR;
						hit = true;
					}else if(hit){
						std::cout << ORUL << LR;
					}else{
						std::cout << OU << " ";
					}
				}
				unsigned int j = 1;
				for (; j <= i; j++){
					std::cout << LR << LR << LR << LR << LR << LR << LR << LR << LR << LR << LR;
				}
				std::cout << LR << LO;
				for (;j < it.second->Out.size(); j++){
					std::cout << "          " << OU;
				}std::cout << std::endl;
				i++;
			}
		} //TODO schöner schreiben
	}
	for (auto&& it : MachineAtlas){
		for (unsigned int i = 0; i < ContainerAtlas.size(); i++){
			std::cout << OU << " ";
		}
		std::cout << "Index: " << it.first << std::endl;
		int i = 0;
		for(; i < it.second.InSecondaryIndex; i++){
			std::cout << OU << " ";
		}
		std::cout << ORU << LR;
		for(; i+1 < (int)ContainerAtlas.size(); i++){
			std::cout << ORUL << LR;
		}
		std::cout << LR << LR << LR << LR << LR << LR << LR << LR << LR << LR << LR << LR << LU << std::endl;
		i = 0;
		for(; i < it.second.InPrimeIndex; i++){
			std::cout << OU << " ";
		}
		std::cout << ORU << LR;
		for(; i+1 < (int)ContainerAtlas.size(); i++){
			std::cout << ORUL << LR;
		}
		std::cout << LR << LU << "          " << OU << std::endl;

		it.second.Render(ContainerAtlas.size());

		i = 0;
		for(; i < it.second.OutPrimeIndex; i++){
			std::cout << OU << " ";
		}
		std::cout << ORU << LR;
		for(; i+1 < (int)ContainerAtlas.size(); i++){
			std::cout << ORUL << LR;
		}
		std::cout << LR << LO << "          " << OU << std::endl;
		i = 0;
		for(; i < it.second.OutSecondaryIndex; i++){
			std::cout << OU << " ";
		}
		std::cout << ORU << LR;
		for(; i+1 < (int)ContainerAtlas.size(); i++){
			std::cout << ORUL << LR;
		}
		std::cout << LR << LR << LR << LR << LR << LR << LR << LR << LR << LR << LR << LR << LO << std::endl;
	}
}

void Blackbox::AddIn(Container *New){
	Container tmp;
	tmp.Amount = New->Amount;
	tmp.Content = New->Content;
	LastContainerIndex++;
	ContainerAtlas[LastContainerIndex] = tmp;
	In[New] = &ContainerAtlas[LastContainerIndex];
}

bool Blackbox::DeleteIn(Container *New){
	if (In.find(New) == In.end()){
		return false;
	}
	In.erase(New);
	return true;
}

void Blackbox::AddOut(Container *New){
	Container tmp;
	tmp.Amount = New->Amount;
	tmp.Content = New->Content;
	LastContainerIndex++;
	ContainerAtlas[LastContainerIndex] = tmp;
	Out[&ContainerAtlas[LastContainerIndex]] = New;
}

bool Blackbox::DeleteOut(Container *New){
	if (Out.find(New) == Out.end()){
		return false;
	}
	Out.erase(New);
	return true;
}
