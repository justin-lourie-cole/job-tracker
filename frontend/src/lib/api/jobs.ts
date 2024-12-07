import fetchClient from "@/lib/api/api-client";
import type { Job } from "@/types/job";

export const DEFAULT_MOCK_JOBS: Job[] = [
	{
		id: 1,
		jobTitle: "Software Engineer",
		company: "Tech Corp",
		location: "Remote",
		industry: "Technology",
		companyOverview: "A tech company",
		whyIWantToWorkHere: "I want to work here because I love technology",
		status: "open",
		dateApplied: new Date().toISOString(),
	},
	{
		id: 2,
		jobTitle: "Product Manager",
		company: "Startup Inc",
		location: "New York, NY",
		industry: "Technology",
		companyOverview: "A tech company",
		whyIWantToWorkHere: "I want to work here because I love technology",
		status: "open",
		dateApplied: new Date().toISOString(),
	},
	{
		id: 3,
		jobTitle: "Data Scientist",
		company: "AI Solutions",
		location: "San Francisco, CA",
		industry: "Artificial Intelligence",
		companyOverview: "Leading AI research and development firm",
		whyIWantToWorkHere: "Passionate about machine learning and AI applications",
		status: "pending",
		dateApplied: new Date().toISOString(),
	},
	{
		id: 4,
		jobTitle: "UX Designer",
		company: "Design Studio",
		location: "Austin, TX",
		industry: "Design",
		companyOverview: "Award-winning design agency",
		whyIWantToWorkHere: "Excited about creating intuitive user experiences",
		status: "rejected",
		dateApplied: new Date().toISOString(),
	},
	{
		id: 5,
		jobTitle: "DevOps Engineer",
		company: "Cloud Systems",
		location: "Seattle, WA",
		industry: "Cloud Computing",
		companyOverview: "Cloud infrastructure solutions provider",
		whyIWantToWorkHere: "Interest in scaling and automating cloud systems",
		status: "interviewing",
		dateApplied: new Date().toISOString(),
	},
];

// Create a jobs API factory that accepts mock data
export const createJobsApi = (mockData: Job[] = DEFAULT_MOCK_JOBS) => ({
	getAll: () => Promise.resolve(mockData),
	create: (job: Partial<Job>) =>
		Promise.resolve({
			...job,
			id: mockData.length + 1,
			createdAt: new Date().toISOString(),
			status: "open",
		} as Job),
});

// Export a default instance with the default mock data
export const jobsApi = createJobsApi();
