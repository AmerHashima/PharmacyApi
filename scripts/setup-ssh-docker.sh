#!/bin/bash

# Script to setup SSH keys for Docker and GitHub integration
echo "?? Setting up SSH keys for Docker and GitHub..."

# Create SSH key if it doesn't exist
if [ ! -f ~/.ssh/id_rsa ]; then
    echo "?? Generating new SSH key..."
    ssh-keygen -t rsa -b 4096 -C "your-email@example.com" -f ~/.ssh/id_rsa -N ""
    echo "? SSH key generated at ~/.ssh/id_rsa"
else
    echo "? SSH key already exists at ~/.ssh/id_rsa"
fi

# Start SSH agent
echo "?? Starting SSH agent..."
eval "$(ssh-agent -s)"

# Add SSH key to agent
echo "?? Adding SSH key to agent..."
ssh-add ~/.ssh/id_rsa

# Display public key for GitHub
echo ""
echo "?? Copy this public key to GitHub:"
echo "   Go to: https://github.com/settings/ssh/new"
echo "   Title: Docker Build Key"
echo ""
echo "?? Public Key:"
cat ~/.ssh/id_rsa.pub
echo ""

# Test SSH connection to GitHub
echo "?? Testing SSH connection to GitHub..."
ssh -T git@github.com

echo ""
echo "? SSH setup complete!"
echo "?? Don't forget to:"
echo "   1. Add the public key to your GitHub account"
echo "   2. Update the image name in docker-compose.yml"
echo "   3. Set up repository secrets if needed"