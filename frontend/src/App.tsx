import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { JobList } from "./features/jobs/JobList";

// Create a client
const queryClient = new QueryClient();

function App() {
	return (
		<QueryClientProvider client={queryClient}>
			<div className="min-h-screen bg-background">
				<header className="border-b">
					<div className="container mx-auto px-4 py-4">
						<h1 className="text-3xl font-bold">Job Application Tracker</h1>
					</div>
				</header>
				<main className="container mx-auto">
					<JobList />
				</main>
			</div>
		</QueryClientProvider>
	);
}

export default App;
