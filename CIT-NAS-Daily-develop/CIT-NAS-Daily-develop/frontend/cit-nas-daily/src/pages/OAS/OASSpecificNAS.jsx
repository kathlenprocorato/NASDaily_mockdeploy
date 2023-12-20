import { Header } from "../../components/Header";
import { SpecificNASTabs } from "../../components/OAS/SpecificNASTabs";

export const OASSpecificNAS = () => {
  return (
    <div>
      <div className="flex items-center">
        <div className="flex-grow">
          <Header />
        </div>
      </div>
      <SpecificNASTabs />
    </div>
  );
};
